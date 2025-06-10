using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TowerVisualRestorer : MonoBehaviour
{
    [Header("Managers")]
    public ProgressManager progressManager;
    public ProgressData progressData;

    [Header("Torre asociada")]
    public Element activeElement;

    [Header("Nubes corruptas")]
    public GameObject[] corruptedClouds;

    [Header("Árboles actuales y nuevos")]
    public GameObject[] trees;
    public Mesh[] newTrees;
    public GameObject[] bigTrees;
    public Mesh[] newBigTrees;

    void Start()
    {
        if (ProgressManager.Instance.Data.towerActiveElements.Contains(activeElement))
        {
            Utils.DestroyCorruptedClouds(corruptedClouds);
            if (activeElement == Element.Earth && trees.Length > 0 && newTrees.Length > 0)
            {
                Utils.ReplaceTrees(trees, newTrees);
                Utils.ReplaceBigTrees(bigTrees, newBigTrees);
                Debug.Log("--->>>> CAMBIANDO ARBOLES");
            }
        }
    }
}
