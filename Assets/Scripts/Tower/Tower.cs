using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Torre")]
    public Element activeElement;
    public int max_life;
    public int life;
    public int invoke_cost;
    public bool isOnCooldown;
    [Header("Zonas de contacto")]
    public bool firstZoneContact;
    public bool secondZoneContact;
    public SecondZone secondZone;

    [Header("Materiales y colores")]
    public Material[] materials;
    public Color[] corruptedColors;
    public Color[] colors;



    [Header("Texto del canva")]
    //public TextMeshProUGUI life_text;
    //[SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public float cooldownTime;


    // Escondemos del inspector porque si no, al eliminar el enemigo de la lista, salta error de que no lo encuentra
    public List<GameObject> enemiesInHealRange = new List<GameObject>();
    public List<GameObject> enemiesInSecondZoneRange = new List<GameObject>();

    [Header("Spawner")]
    public Spawner spawner;

    //[Header("Elemental Objects Assigned")]
    //public Transform elementalObjects;
    //public ChangeAppearence changeAppearence;
    //public Color healthyTreeColor = new Color();

    //[Header("CameraManager")]
    //public CameraManager cameraManager;

    [Header("Progress Manager")]
    public ProgressManager progressManager;
    public ProgressData progressData;

    [Header("TowerHud")]
    public TowerHUD towerHUD;


    /* ORDEN DE LAS TORRES (Constants.cs) */
    /* 
        None ===== [0]
        Earth ==== [1]
        Water ==== [2]
        Fire ===== [3]
        Electric = [4]
    */

    void Start()
    {
        this.life = max_life;
    }

    void Update()
    {
        /* if (life_text != null) // Mostrar la vida de la torre por pantalla
        {
            life_text.text = "T.Life: " + life;
        } */

        if (Input.GetKeyDown(KeyCode.L)) // Restar vida
        {
            life = life - 5;
            towerHUD.UpdateHealth(life);
        }

        if (Input.GetKeyDown(KeyCode.P)) // Restar vida
        {
            life += 5;
            towerHUD.UpdateHealth(life);
            if (life > max_life)
            {
                life = max_life;
            }
        }

        if (Input.GetKeyDown(KeyCode.M)) // Restar vida
        {
            Utils.ReplaceMaterials(materials, colors);
            Debug.Log("------- RESTAURANDO COLORES POR DEFECTO");
        }

        if (ProgressManager.Instance.Data.towerActiveElements.Contains(activeElement))
        {
            int position = ProgressManager.Instance.Data.towerActiveElements.IndexOf(activeElement); // Para obtener la posicion de la torre en el array del JSON
                                                                                                     //Debug.Log("POSITION EN EL ARRAY: " + position);
            Utils.ReplaceMaterials(materials, corruptedColors);
            Destroy(gameObject);
        }

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
                            Debug.Log("-----> ACERCATE");
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

    /* public void DestroyTower()
    {
        gameObject.SetActive(false);
        //cameraManager.ActivateFade();
        Invoke(nameof(EraseTower), 1.0f);
    } */

    /* public void EraseTower()
    {
        Utils.ReplaceMaterials(materials, corruptedColors);
        Destroy(gameObject); // Destruye la torre si se queda sin vida
        ProgressManager.Instance.Data.towerActiveElements.Add(activeElement);
        //Debug.Log("TORRES EN EL PROGRESS DATA: " + string.Join(", ", ProgressManager.Instance.Data.towerActiveElements));
        progressManager.SaveGame();
    } */

    public void DestroyTower()
    {
        Utils.ReplaceMaterials(materials, corruptedColors);
        Destroy(transform.root.gameObject); // Destruye la torre si se queda sin vida
        ProgressManager.Instance.Data.towerActiveElements.Add(activeElement);
        progressManager.SaveGame();
    }

    public void IncreaseDecreaseTowerLife(bool increase, int life)
    {
        if (increase)
        {
            this.life += 1; // Incrementamos el valor de la vida

            if (this.life > max_life) // Si se pasa del limite de vida establecido, se rebaja hasta se vida maxima
            {
                this.life = max_life;
                Debug.Log("---> Curacion aplicado");
            }
        }
        else
        {
            this.life -= invoke_cost;
            Debug.Log("---> Coste aplicado");
        }
    }

    private void DestroyEnemy()
    {
        GameObject enemy = enemiesInHealRange[0]; // Creamos referencia del prefab guardado en X posicion del array
        enemiesInHealRange.RemoveAt(0); // Eliminamos ese prefab del array
        enemiesInSecondZoneRange.Remove(enemy.GetComponent<Transform>().gameObject);
        Destroy(enemy.transform.root.gameObject); // Destruimos el prefab
        secondZone.enemyCount -= 2;
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
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy)) // Comparamos el tag 
        {
            Enemy enemy = other.GetComponent<Enemy>(); // Cogemos su componente enemy
            if (enemy != null)
            {
                if (enemy.activeElement == activeElement) // Verificamos si es del mismo tipo que la torre
                {
                    Debug.Log("----->>>>>>>>>> DETECTO ALGO DE ENEMY");
                    if (!enemiesInHealRange.Contains(other.gameObject)) // Si el enemigo no esta en el array de enemigos en zona, lo añadimos
                    {
                        enemiesInHealRange.Add(other.GetComponent<Transform>().gameObject); // Añadimos el enemigo a la lista de enemigos detectados en la zona de curacion
                    }
                }
            }
            else
            {
                Debug.Log("---->>>>> NO ENTRA EN ZONA CURA");
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == activeElement)
                {
                    firstZoneContact = false;
                    enemiesInHealRange.Remove(other.GetComponent<Transform>().gameObject); // Eliminamos el enemigo que sale de la zona de curacion
                    Debug.Log("---->>>>> SALE EL ENEMIGO");
                }
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            Enemy enemy = other.GetComponent<Enemy>();
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
    }

}
