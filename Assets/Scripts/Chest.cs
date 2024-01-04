using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using UnityEngine;
using Random = UnityEngine.Random;


public class Chest : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private RelicScriptableObject relicScriptableObject;
    public int GetPrice()
    {
        return price;
    }

    public RelicTypes GetRandomRelic()
    {
        List<RelicTypes> relicTypesList = relicScriptableObject.GetRelicTypesList();
        int randomNumber = Random.Range(0, relicTypesList.Count);
        return relicTypesList[randomNumber];
    }
}
