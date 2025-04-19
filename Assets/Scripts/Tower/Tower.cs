using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum elementType { fire, ice, ray, rock }
public class Tower : MonoBehaviour
{
    [Header("Torre")]
    public int max_life;
    public int life;
    public int invoke_cost;
    public bool isOnCooldown;
    [Header("Zonas de contacto")]
    public bool firstZoneContact;
    public bool secondZoneContact;
    public SecondZone secondZone;



    public elementType selectedType;


    [Header("Texto del canva")]
    public TextMeshProUGUI life_text;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public float cooldownTime;


     // Escondemos del inspector porque si no, al eliminar el enemigo de la lista, salta error de que no lo encuentra
    public List<GameObject> enemiesInHealRange = new List<GameObject>();

    [Header("Spawner")]
    public Spawner spawner;

    [Header("Elemental Objects Assigned")]
    public Transform elementalObjects;
    public ChangeAppearence changeAppearence;
    public Color healthyTreeColor = new Color();

    [Header("CameraManager")]
    public CameraManager cameraManager;
    
    void Start()
    {
        this.life = max_life;
    }

    void Update()
    {
        
        
        if(life_text != null) // Mostrar la vida de la torre por pantalla
        {
            life_text.text = "T.Life: " + life;
        }
        
        if(Input.GetKeyDown(KeyCode.L)) // Restar vida
        {
            life = life - 5;
        }

        if(Input.GetKeyDown(KeyCode.P)) // Restar vida
        {
            life = life + 5;
            if(life > max_life)
            {
                life = max_life;
            }
        }






        
        if(life <= 0)
        {
            Destroy(gameObject); // Destruye la torre si se queda sin vida
            cameraManager.ActivateFade();
            changeAppearence.ToggleColor(elementalObjects, healthyTreeColor);
        }
        else
        {
            if(isOnCooldown) // Si no tengo el cooldown activado
            {   
                CalculateCooldown(); // Se inicia la cuenta atras
            }
            else
            {
                if(life < max_life / 2) // Si la vida es inferior a X
                {
                    if(firstZoneContact)  // Toca el collider interno?
                    {
                        IncreaseDecreaseTowerLife(true, life); // Incremento vida
                        DestroyEnemy(); // Sacrificamos enemigo
                        secondZoneContact = false;
                        firstZoneContact = false;
                        ActivateCooldown();
                    }
                    else 
                    {
                        if(secondZoneContact) // El enemigo de mi tipo esta en el collider exterior?
                        {
                            Debug.Log("-----> ACERCATE");
                        }
                        else 
                        {
                            if(/* life > (life - invoke_cost) */ (life - invoke_cost) > 0)
                            {
                                spawner.SpawnEnemy(0); // Invocamos
                                IncreaseDecreaseTowerLife(false, life); // Aplicamos coste de invocacion
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
                    if(!secondZoneContact && !firstZoneContact) // Si el enemigo no esta en el collider exterior...
                    {
                        if(/* life > (life - invoke_cost) */ (life - invoke_cost) > 0)
                        {
                            spawner.SpawnEnemy(0); // Invoco en el collider exterior, no en el de contacto
                            IncreaseDecreaseTowerLife(false, life);
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

    public void IncreaseDecreaseTowerLife(bool increase, int life)
    {
        if(increase)
        {
            this.life += 1; // Incrementamos el valor de la vida

            if(this.life > max_life) // Si se pasa del limite de vida establecido, se rebaja hasta se vida maxima
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
        Destroy(enemy); // Destruimos el prefab
        secondZone.enemyCount = secondZone.enemyCount - 2; 
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
        timerText.text = string.Format("{0:00}:{1:00}", min, sec);

        if(sec == 0)
        {
            isOnCooldown = false;
            remainingTime = cooldownTime;
            return;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) // Comparamos el tag 
        {   
            TowerEnemy enemy = other.GetComponent<TowerEnemy>(); // Cogemos su componente enemy
            if(enemy != null)
            {
                if (enemy.selectedType == selectedType) // Verificamos si es del mismo tipo que la torre
                {
                    //secondZoneContact = true;
                    // firstZoneContact = true;
                    if (!enemiesInHealRange.Contains(other.gameObject)) // Si el enemigo no esta en el array de enemigos en zona, lo añadimos
                    {
                        enemiesInHealRange.Add(other.gameObject); // Añadimos el enemigo a la lista de enemigos detectados en la zona de curacion
                    }                 
                }
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {   
            TowerEnemy enemy = other.GetComponent<TowerEnemy>();
            if(enemy != null)
            {
                if (enemy.selectedType == selectedType)
                {
                    //secondZoneContact = false;
                    firstZoneContact = false;
                    enemiesInHealRange.Remove(other.gameObject); // Eliminamos el enemigo que sale de la zona de curacion
                }
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {   
            TowerEnemy enemy = other.GetComponent<TowerEnemy>();
            if(enemy != null)
            {
                if (enemy.selectedType == selectedType)
                {
                    firstZoneContact = true;
                }
            }
        }
    }
}
