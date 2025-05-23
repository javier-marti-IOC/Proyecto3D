using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InfoPanelHUD infoPanelHUD;
    // Enemies Prefabs
    public GameObject EarthEnemy;
    public GameObject WaterEnemy;
    public GameObject FireEnemy;
    public GameObject ElectricEnemy;

    // Enemies Spawnes
    public List<Transform> earthEnemiesSpawnersPositions;
    public List<Transform> waterEnemiesSpawnersPositions;
    public List<Transform> fireEnemiesSpawnersPositions;
    public List<Transform> electricEnemiesSpawnersPositions;
    public Transform earthTowerSpawnersPositions;
    public Transform waterTowerSpawnersPositions;
    public Transform fireTowerSpawnersPositions;
    public Transform electricTowerSpawnersPositions;

    // Get Level
    public int earthLevel = 1;
    public int waterLevel = 1;
    public int fireLevel = 1;
    public int electricLevel = 1;
    private List<Element> elementsLevel;
    private bool earthLeveled;
    private bool waterLeveled;
    private bool fireLeveled;
    private bool electricLeveled;

    // Set Level
    private GameObject[] enemies;

    private GameObject[] drops;

    void Awake()
    {
        earthLevel = 1;
        waterLevel = 1;
        fireLevel = 1;
        electricLevel = 1;
        EnemiesGenerator(Element.None);
    }
    public int DamageCalulator(Element dealerElement, int dealerBasicDamage, int dealerElementalDamege, Element takerElement)
    {
        float basicDamageRange = Random.Range(dealerBasicDamage * 0.85f, dealerBasicDamage * 1.15f);
        float elementDamageRange = Random.Range(dealerElementalDamege * 0.85f, dealerElementalDamege * 1.15f);
        switch (ElementInteraction(dealerElement, takerElement))
        {
            case 1:
                return (int)(basicDamageRange + (elementDamageRange * 1.5f));
            case 0:
                return (int)(basicDamageRange + elementDamageRange);
            case -1:
                return (int)(basicDamageRange + (elementDamageRange * 0.75f));
        }
        return 0;
    }
    public int ElementInteraction(Element element1, Element element2)
    {
        if (element1 == Element.Earth && element2 == Element.Electric)
        {
            return 1;
        }
        else if (element1 == Element.Water && element2 == Element.Fire)
        {
            return 1;
        }
        else if (element1 == Element.Fire && element2 == Element.Earth)
        {
            return 1;
        }
        else if (element1 == Element.Electric && element2 == Element.Water)
        {
            return 1;
        }
        else if (element1 == Element.Earth && element2 == Element.Fire)
        {
            return -1;
        }
        else if (element1 == Element.Water && element2 == Element.Electric)
        {
            return -1;
        }
        else if (element1 == Element.Fire && element2 == Element.Water)
        {
            return -1;
        }
        else if (element1 == Element.Electric && element2 == Element.Earth)
        {
            return -1;
        }
        return 0;
    }

    public Element getCounterElement(Element element)
    {
        if (element == Element.None)
        {
            int num = Random.Range(1, 5);
            if (num == 1) return Element.Fire;
            else if (num == 2) return Element.Electric;
            else if (num == 3) return Element.Water;
            else return Element.Earth;
        }
        else if (element == Element.Earth)
        {
            return Element.Fire;
        }
        else if (element == Element.Water)
        {
            return Element.Electric;
        }
        else if (element == Element.Fire)
        {
            return Element.Water;
        }
        else
        {
            return Element.Earth;
        }
    }

    public void EnemiesGenerator(Element element)
    {
        for (int i = earthEnemiesSpawnersPositions.Count - 1; i >= 0; i--)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(EarthEnemy, earthEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
            }
        }
        for (int i = waterEnemiesSpawnersPositions.Count - 1; i >= 0; i--)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(WaterEnemy, waterEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
            }
        }
        for (int i = fireEnemiesSpawnersPositions.Count - 1; i >= 0; i--)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(FireEnemy, fireEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
            }
        }
        for (int i = electricEnemiesSpawnersPositions.Count - 1; i >= 0; i--)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(ElectricEnemy, electricEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
            }
        }
        if (element != Element.Earth)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(EarthEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(WaterEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(FireEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(ElectricEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
            }
        }
        if (element != Element.Water)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(EarthEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(WaterEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(FireEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(ElectricEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
            }
        }
        
        if (element != Element.Fire)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(EarthEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(WaterEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(FireEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(ElectricEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
            }
        }

        if (element != Element.Electric)
        {
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(EarthEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(WaterEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(FireEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
            }
            for (int x = Random.Range(0, 2); x >= 0; x--)
            {
                Instantiate(ElectricEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
            }
        }

        if (FindObjectOfType<ProgressManager>().LoadData())
        {
            elementsLevel = FindObjectOfType<ProgressManager>().progressData.GetTowerActiveElements();
            earthLeveled = false;
            waterLeveled = false;
            fireLeveled = false;
            electricLeveled = false;
            for (int i = 0; i < elementsLevel.Count; i++)
            {
                if (elementsLevel[i] == Element.Earth)
                {
                    earthLevel = i + 1;
                    earthLeveled = true;
                }
                else if (elementsLevel[i] == Element.Water)
                {
                    waterLevel = i + 1;
                    waterLeveled = true;
                }
                else if (elementsLevel[i] == Element.Fire)
                {
                    fireLevel = i + 1;
                    fireLeveled = true;
                }
                else if (elementsLevel[i] == Element.Electric)
                {
                    electricLevel = i + 1;
                    electricLeveled = true;
                }
                if (!earthLeveled) earthLevel = i + 2;
                if (!waterLeveled) waterLevel = i + 2;
                if (!fireLeveled) fireLevel = i + 2;
                if (!electricLeveled) electricLevel = i + 2;
            }
        }
        enemies = GameObject.FindGameObjectsWithTag(Constants.enemy);
        foreach (GameObject e in enemies)
        {
            if (e.GetComponent<Enemy>().activeElement == Element.Earth)
            {
                e.GetComponent<Enemy>().enemyLevel = earthLevel;
            }
            else if (e.GetComponent<Enemy>().activeElement == Element.Water)
            {
                e.GetComponent<Enemy>().enemyLevel = waterLevel;
            }
            else if (e.GetComponent<Enemy>().activeElement == Element.Fire)
            {
                e.GetComponent<Enemy>().enemyLevel = fireLevel;
            }
            else if (e.GetComponent<Enemy>().activeElement == Element.Electric)
            {
                e.GetComponent<Enemy>().enemyLevel = electricLevel;
            }
            e.GetComponent<Enemy>().SetStatsByLevel();
        }
    }
    public void ResetEnemies(Element element)
    {
        infoPanelHUD.ShowText("Els altres elements han guanyat poder");
        enemies = GameObject.FindGameObjectsWithTag(Constants.enemy);
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].GetComponent<Enemy>().Dying(false);
        }
        EnemiesGenerator(element);
    }
}
