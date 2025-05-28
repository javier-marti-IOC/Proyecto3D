using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public GameObject secondZoneObject;
    [Header("Torre")]
    public Element activeElement;
    public int max_life;
    public int life;
    public int invoke_cost;
    public bool isOnCooldown;
    [SerializeField] float remainingTime;
    public float cooldownTime;
    private float regenTimer = 0f;

    [Header("Zonas de contacto")]
    public bool firstZoneContact;
    public bool secondZoneContact;
    public SecondZone secondZone;

    [Header("Materiales y colores")]
    public Material[] materials;
    public Color[] corruptedColors;
    public Color[] colors;
    public GameObject deathTowerParticles;
    public GameObject[] environmentParticles;
    public GameObject[] trees;
    public Mesh[] newTrees;
    public GameObject[] corruptedClouds;
    public GameObject oppositeOrbSpawnGenerator;



    // Escondemos del inspector porque si no, al eliminar el enemigo de la lista, salta error de que no lo encuentra
    public List<GameObject> enemiesInHealRange = new List<GameObject>();
    public List<GameObject> enemiesInSecondZoneRange = new List<GameObject>();

    [Header("Spawner")]
    public Spawner spawner;

    [Header("Managers")]
    public ProgressManager progressManager;
    public ProgressData progressData;
    public GameManager gameManager;

    [Header("TowerHud")]
    public TowerHUD towerHUD;

    [Header("CameraSwitcher")]
    public CameraFadeSwitcher cameraFadeSwitcher;

    void Start()
    {
        this.life = max_life;
        if (ProgressManager.Instance.Data.towerActiveElements.Contains(activeElement))
        {
            Utils.ReplaceMaterials(materials, colors);
            ChangeEnvironmentParticles();
            Utils.DestroyCorruptedClouds(corruptedClouds);
            if (activeElement == Element.Earth && trees.Length > 0 && newTrees.Length > 0)
            {
                Utils.ReplaceTrees(trees, newTrees);
                Debug.Log("--->>>> CAMBIANDO ARBOLES");
            }
            Destroy(transform.parent.gameObject);
            if (oppositeOrbSpawnGenerator != null)
            {    
                if (oppositeOrbSpawnGenerator.activeSelf)
                {
                    Destroy(oppositeOrbSpawnGenerator);
                }
            }
        }
        else
        {
            Utils.ReplaceMaterials(materials, corruptedColors);
            ChangeEnvironmentParticlesOff();
            secondZoneObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Restar vida
        {
            life = life - 5;
            towerHUD.UpdateHealth(life);
        }

        if (Input.GetKeyDown(KeyCode.P)) // Sumar vida
        {
            life += 5;
            towerHUD.UpdateHealth(life);
            if (life > max_life)
            {
                life = max_life;
            }
        }

        if (Input.GetKeyDown(KeyCode.M)) // Reiniciar materiales
        {
            Utils.ReplaceMaterials(materials, corruptedColors);
            ChangeEnvironmentParticlesOff();
            Debug.Log("------- RESTAURANDO COLORES POR DEFECTO");
        }

        if (!secondZone.playerInSecondZoneRange && life != max_life)
        {
            IncreaseLifeProgressively();
        }

        // ***************************************************
        // ***************************************************
        // ********************  TOWER BT ********************
        // ***************************************************
        // ***************************************************

        if (life <= 0)
        {
            DestroyTower();
        }
        else
        {
            if (isOnCooldown) // Si no tengo el cooldown activado
            {
                CalculateCooldown(); // Se inicia la cuenta atras
            }
            else
            {
                if (life < max_life / 2) // Si la vida es inferior a X
                {
                    if (firstZoneContact)  // Toca el collider interno?
                    {
                        IncreaseDecreaseTowerLife(true, life); // Incremento vida
                        towerHUD.UpdateHealth(life);
                        DestroyEnemy(); // Sacrificamos enemigo
                        UncallAllEnemies(enemiesInSecondZoneRange);
                        secondZoneContact = false;
                        firstZoneContact = false;
                        ActivateCooldown();
                    }
                    else
                    {
                        if (secondZoneContact) // El enemigo de mi tipo esta en el collider exterior?
                        {
                            // Debug.Log("-----> ACERCATE");
                            CallAllEnemies(enemiesInSecondZoneRange);
                        }
                        else
                        {
                            if (/* life > (life - invoke_cost) */ (life - invoke_cost) > 0)
                            {
                                spawner.SpawnEnemy(activeElement); // Invocamos
                                IncreaseDecreaseTowerLife(false, life); // Aplicamos coste de invocacion
                                towerHUD.UpdateHealth(life);
                            }
                            else
                            {
                                Debug.Log("*** Si invoco, muero por el coste ***");
                            }
                        }
                    }
                }
                else
                {
                    if (!secondZoneContact && !firstZoneContact) // Si el enemigo no esta en el collider exterior...
                    {
                        if (/* life > (life - invoke_cost) */ (life - invoke_cost) > 0)
                        {
                            spawner.SpawnEnemy(activeElement); // Invoco en el collider exterior, no en el de contacto
                            IncreaseDecreaseTowerLife(false, life);
                            towerHUD.UpdateHealth(life);
                        }
                        else
                        {
                            Debug.Log("*** Si invoco, muero por el coste ***");
                        }
                    }
                }
            }
        }
    }

    public void IncreaseLifeProgressively()
    {
        regenTimer += Time.deltaTime;

        if (regenTimer >= 1f)
        {
            regenTimer = 0f;
            life += 1;

            if (life > max_life)
                life = max_life;

            towerHUD.UpdateHealth(life);
        }
        // Debug.Log("---->>>> INCREMENTANDO VIDA PROGRESIVAMENTE");
    }

    public void DestroyTower()
    {
        for (int i = enemiesInSecondZoneRange.Count - 1; i >= 0; i--)
        {
            enemiesInSecondZoneRange[i].GetComponent<Enemy>().Dying(activeElement != Element.None);
        }
        InstantiateDeathTowerParticles();
        Utils.ReplaceMaterials(materials, colors);
        Utils.DestroyCorruptedClouds(corruptedClouds);
        if (activeElement == Element.Earth && trees.Length > 0 && newTrees.Length > 0)
        {
            Utils.ReplaceTrees(trees, newTrees);
            // Debug.Log("--->>>> CAMBIANDO ARBOLES");
        }
        if (activeElement == Element.None)
        {
            ProgressManager.Instance.Data.tutorial = true;
            progressManager.SaveGame();
            // Debug.Log("---->>>>>>>>>>>>>> TORRE DEL TUTORIAL DESTRUIDA");
        }
        AudioManager.Instance?.Play("RestoreElement");
        ChangeEnvironmentParticles();
        ProgressManager.Instance.Data.towerActiveElements.Add(activeElement);
        // if (activeElement == Element.Earth)
        // {
        //     progressManager.earthTowerDestroyed = true;
        // }
        // else if (activeElement == Element.Fire)
        // {
        //     progressManager.fireTowerDestroyed = true;
        // }
        // else if (activeElement == Element.Water)
        // {
        //     progressManager.waterTowerDestroyed = true;
        // }
        // else if(activeElement == Element.Electric)
        // {
        //     progressManager.electricTowerDestroyed = true;
        // }
        progressManager.SaveGame();
        progressManager.checkDestroyedTowers();
        gameManager.ResetEnemies(activeElement);
        if (oppositeOrbSpawnGenerator != null)
        {    
            if (oppositeOrbSpawnGenerator.activeSelf)
            {
                Destroy(oppositeOrbSpawnGenerator);
            }
        }
        if (cameraFadeSwitcher != null)
        {
            cameraFadeSwitcher.SwitchCameraWithFade();
        }
        else
        {
            Debug.Log("---->>>> NO EXISTE EL CAMERA FADE SWITCHER");
        }
        Destroy(transform.parent.gameObject); // Destruye la torre si se queda sin vida
    }

    public void IncreaseDecreaseTowerLife(bool increase, int life)
    {
        if (increase)
        {
            this.life += 20; // Incrementamos el valor de la vida

            if (this.life > max_life) // Si se pasa del limite de vida establecido, se rebaja hasta se vida maxima
            {
                this.life = max_life;
                // Debug.Log("---> Curacion aplicado");
            }
        }
        else
        {
            this.life -= invoke_cost;
            // Debug.Log("---> Coste aplicado");
        }
    }

    private void DestroyEnemy()
    {
        GameObject enemy = enemiesInHealRange[0]; // Creamos referencia del prefab guardado en X posicion del array
        enemiesInHealRange.RemoveAt(0); // Eliminamos ese prefab del array
        enemiesInSecondZoneRange.Remove(enemy.GetComponent<Transform>().gameObject);
        enemy.GetComponent<Enemy>().Dying(false); // Destruimos el prefab
        CheckSecondZoneCount(enemiesInSecondZoneRange);
    }

    public void CheckSecondZoneCount(List<GameObject> enemiesInSecondZoneRange)
    {
        if (enemiesInSecondZoneRange.Count > 0)
        {
            secondZoneContact = true;
        }else
        {
            secondZoneContact = false;
        }
    }


    public void ActivateCooldown()
    {
        isOnCooldown = true;
    }

    private void CalculateCooldown() // Funcion para el cooldown
    {
        remainingTime -= Time.deltaTime;
        int min = Mathf.FloorToInt(remainingTime / 60);
        int sec = Mathf.FloorToInt(remainingTime % 60);
        //timerText.text = string.Format("{0:00}:{1:00}", min, sec);

        if (sec == 0)
        {
            isOnCooldown = false;
            remainingTime = cooldownTime;
            return;
        }
    }
    public void OnTriggerHealEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy)) // Comparamos el tag 
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>(); // Cogemos su componente enemy
            if (enemy != null)
            {
                if (enemy.activeElement == activeElement) // Verificamos si es del mismo tipo que la torre
                {
                    // Debug.Log("----->>>>>>>>>> DETECTO ALGO DE ENEMY");
                    if (!enemiesInHealRange.Contains(other.gameObject)) // Si el enemigo no esta en el array de enemigos en zona, lo añadimos
                    {
                        // Debug.Log("HEALER");
                        enemiesInHealRange.Add(other.gameObject.GetComponent<Transform>().gameObject); // Añadimos el enemigo a la lista de enemigos detectados en la zona de curacion
                    }
                }
            }
            else
            {
                // Debug.Log("---->>>>> NO ENTRA EN ZONA CURA");
            }
        }
    }
    public void OnTriggerHealExit(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == activeElement)
                {
                    firstZoneContact = false;
                    enemiesInHealRange.Remove(other.gameObject); // Eliminamos el enemigo que sale de la zona de curacion
                    // Debug.Log("---->>>>> SALE EL ENEMIGO");
                }
            }
        }
    }

    public void OnTriggerHealStay(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == activeElement)
                {
                    firstZoneContact = true;
                }
            }
        }
    }
    // Activamos el towerCalling para todos los enemigos instanciados y dentro de la segunda zona
    public void CallAllEnemies(List<GameObject> instantiatedEnemies)
    {
        for (int i = instantiatedEnemies.Count - 1; i >= 0; i--)
        {
            if (instantiatedEnemies[i] == null)
            {
                instantiatedEnemies.RemoveAt(i); // Limpiamos la lista de referencias destruidas
            }
            else
            {
                Enemy enemy = instantiatedEnemies[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.towerCalling = true;
                }
            }
        }
    }


    // Desactivamos el towerCalling para todos los enemigos instanciados y dentro de la segunda zona, para que vuelvan a patrullar
    public void UncallAllEnemies(List<GameObject> instantiatedEnemies)
    {
        for (int i = instantiatedEnemies.Count - 1; i >= 0; i--)
        {
            if (instantiatedEnemies[i] == null)
            {
                instantiatedEnemies.RemoveAt(i);
            }
            else
            {
                Enemy enemy = instantiatedEnemies[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.towerCalling = false;
                }
            }
        }
    }


    public void HealthTaken(int damage)
    {
        life -= damage;
        towerHUD.UpdateHealth(life);
    }

    public void ChangeEnvironmentParticles()
    {
        foreach (GameObject particle in environmentParticles)
        {
            particle.SetActive(true);
        }
    }

    public void ChangeEnvironmentParticlesOff()
    {
        foreach (GameObject particle in environmentParticles)
        {
            particle.SetActive(false);
        }
    }

    private void InstantiateDeathTowerParticles()
    {
        if (deathTowerParticles != null)
        {
            Instantiate (deathTowerParticles, transform.position, quaternion.identity);
        }
    }

}
