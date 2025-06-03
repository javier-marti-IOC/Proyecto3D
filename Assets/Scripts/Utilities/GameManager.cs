using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //BoxInfo
    public InfoPanelHUD infoPanelHUD;
    private bool firstTimeEnterDPADHelp;
    private bool firstTimeExitDPADHelp;
    private bool isTutorial;
    private bool firstTimeSlashAttackEnterHelp;
    private bool firstTimeSlashAttackExitHelp;
    private bool firstTimeEnterRollHelp;
    private bool firstTimeExitRollHelp;
    private bool firstTimeEnterAttackHelp;
    private bool firstTimeExitAttackHelp;
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
    public int[] DamageCalulator(Element dealerElement, int dealerBasicDamage, int dealerElementalDamege, Element takerElement)
    {
        float basicDamageRange = Random.Range(dealerBasicDamage * 0.85f, dealerBasicDamage * 1.15f);
        float elementDamageRange = Random.Range(dealerElementalDamege * 0.85f, dealerElementalDamege * 1.15f);
        switch (ElementInteraction(dealerElement, takerElement))
        {
            case 1:
                return new int[] { (int)basicDamageRange, (int)(elementDamageRange * 2f) };
            case 0:
                return new int[] { (int)basicDamageRange, (int)elementDamageRange };
            case -1:
                return new int[] { (int)basicDamageRange, (int)(elementDamageRange * 0.5f) };
        }
        return new int[] { 0, 0 };
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
            Instantiate(EarthEnemy, earthEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
        }
        for (int i = waterEnemiesSpawnersPositions.Count - 1; i >= 0; i--)
        {
            Instantiate(WaterEnemy, waterEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
        }
        for (int i = fireEnemiesSpawnersPositions.Count - 1; i >= 0; i--)
        {
            Instantiate(FireEnemy, fireEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
        }
        for (int i = electricEnemiesSpawnersPositions.Count - 1; i >= 0; i--)
        {
            Instantiate(ElectricEnemy, electricEnemiesSpawnersPositions[i].position, Quaternion.identity, null);
        }
        if (element != Element.Earth)
        {
            Instantiate(EarthEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(WaterEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(FireEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(ElectricEnemy, earthTowerSpawnersPositions.position, Quaternion.identity, null);
        }
        if (element != Element.Water)
        {
            Instantiate(EarthEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(WaterEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(FireEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(ElectricEnemy, waterTowerSpawnersPositions.position, Quaternion.identity, null);
        }

        if (element != Element.Fire)
        {
            Instantiate(EarthEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(WaterEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(FireEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(ElectricEnemy, fireTowerSpawnersPositions.position, Quaternion.identity, null);
        }

        if (element != Element.Electric)
        {
            Instantiate(EarthEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(WaterEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(FireEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
            Instantiate(ElectricEnemy, electricTowerSpawnersPositions.position, Quaternion.identity, null);
        }

        if (FindObjectOfType<ProgressManager>().LoadData())
        {
            Debug.Log("GameManger HaveData");
            elementsLevel = FindObjectOfType<ProgressManager>().progressData.GetTowerActiveElements();
            earthLeveled = false;
            waterLeveled = false;
            fireLeveled = false;
            electricLeveled = false;
            for (int i = 0; i < elementsLevel.Count; i++)
            {
                if (elementsLevel[i] == Element.Earth)
                {
                    earthLevel = i;
                    earthLeveled = true;
                }
                else if (elementsLevel[i] == Element.Water)
                {
                    waterLevel = i;
                    waterLeveled = true;
                }
                else if (elementsLevel[i] == Element.Fire)
                {
                    fireLevel = i;
                    fireLeveled = true;
                }
                else if (elementsLevel[i] == Element.Electric)
                {
                    electricLevel = i;
                    electricLeveled = true;
                }
                if (!earthLeveled) earthLevel = i + 1;
                if (!waterLeveled) waterLevel = i + 1;
                if (!fireLeveled) fireLevel = i + 1;
                if (!electricLeveled) electricLevel = i + 1;
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
        if (element != Element.None)
        {
            infoPanelHUD.ShowText("Els altres elements han guanyat poder.");
        }
        enemies = GameObject.FindGameObjectsWithTag(Constants.enemy);
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].GetComponent<Enemy>().Dying(false);
        }
        EnemiesGenerator(element);
    }
    public void ResetEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag(Constants.enemy);
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].GetComponent<Enemy>().Dying(false);
        }
        EnemiesGenerator(Element.None);
    }


    public void EnterRollHelp()
    {
        Debug.Log(1);
        if (!firstTimeEnterRollHelp)
        {
            Debug.Log(2);
            firstTimeEnterRollHelp = true;
            isTutorial = true;
            infoPanelHUD.EnterText("Utilitza 'B' per rodar o esquivar.");
        }
    }

    public void ExitRollHelp()
    {
        if (!firstTimeExitRollHelp && firstTimeEnterRollHelp && isTutorial)
        {
            firstTimeExitRollHelp = true;
            infoPanelHUD.HideText();
        }
    }

    public void EnterAttackHelp()
    {
        if (!firstTimeExitRollHelp && isTutorial)
        {
            firstTimeExitRollHelp = true;
            infoPanelHUD.HideText();
        }
        if (!firstTimeEnterAttackHelp && isTutorial)
        {
            firstTimeEnterAttackHelp = true;
            infoPanelHUD.EnterText("Utilitza 'RB' o 'RT' per Atacar a un enemic.");
        }
    }

    public void ExitAttackHelp()
    {
        if (!firstTimeExitAttackHelp && firstTimeEnterAttackHelp && isTutorial)
        {
            firstTimeExitAttackHelp = true;
            infoPanelHUD.HideText();
        }
    }


    public void EnterDPADHelp()
    {
        if (!firstTimeExitAttackHelp && isTutorial)
        {
            firstTimeExitAttackHelp = true;
            infoPanelHUD.HideText();
        }
        if (!firstTimeEnterDPADHelp && isTutorial)
        {
            firstTimeEnterDPADHelp = true;
            infoPanelHUD.EnterText("Utilitza la creueta per canviar l'element.");
        }
    }

    public void ExitDPADHelp()
    {
        if (!firstTimeExitDPADHelp && isTutorial && firstTimeEnterDPADHelp)
        {
            firstTimeExitDPADHelp = true;
            infoPanelHUD.HideText();
        }
    }

    public void EnterSlashAttackHelp()
    {
        if (!firstTimeSlashAttackEnterHelp && isTutorial && firstTimeExitDPADHelp)
        {
            firstTimeSlashAttackEnterHelp = true;
            infoPanelHUD.EnterText("Utilitza 'RT' per utilitzar l'atac fort i vençer l'altar.");
        }
    }

    public void ExitSlashAttackHelp()
    {
        if (!firstTimeSlashAttackExitHelp && isTutorial && firstTimeSlashAttackEnterHelp)
        {
            firstTimeSlashAttackExitHelp = true;
            infoPanelHUD.HideText();
        }
    }
    
}
