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
    private const float CommonPossibilities = 80;
    private const float RarePossibilities = 18;
    private const float LegendaryPossibilities = 2;
    
    public int GetPrice()
    {
        return price;
    }

    public RelicTypes GetRandomRelic()
    {
        float randomRarity = Random.Range(0,100);
        Debug.Log("kac attık reis "+randomRarity);
        if (randomRarity < LegendaryPossibilities)
        {
            return GetRandomLegendaryRelic(); 
        }
        else if (randomRarity is > LegendaryPossibilities and < RarePossibilities)
        {
            return GetRandomRareRelic(); 
        }
        else
        {
            return GetRandomCommonRelic(); 
        }
    }

    private RelicTypes GetRandomLegendaryRelic()
    {
        List<RelicTypes> relicTypesList = relicScriptableObject.GetLegendaryRelics();
        int randomNumber = Random.Range(0, relicTypesList.Count);
        return relicTypesList[randomNumber];
    }
    
    private RelicTypes GetRandomRareRelic()
    {
        List<RelicTypes> relicTypesList = relicScriptableObject.GetRareRelics();
        int randomNumber = Random.Range(0, relicTypesList.Count);
        return relicTypesList[randomNumber];
    }
    
    private RelicTypes GetRandomCommonRelic()
    {
        List<RelicTypes> relicTypesList = relicScriptableObject.GetCommonRelics();
        int randomNumber = Random.Range(0, relicTypesList.Count);
        return relicTypesList[randomNumber];
    }
    
}
