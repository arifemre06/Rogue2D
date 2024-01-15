using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RightItemShop : MonoBehaviour
{
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private RelicScriptableObject relicScriptableObject;
    [SerializeField] private float priceIncreaseModifier;
    [SerializeField] private int price;
    private bool _canPurchase;

    private void Awake()
    {
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
    }

    private void Start()
    {
        _canPurchase = true;
        priceText.text = price.ToString();
    }

    private void OnPurchaseButtonClicked()
    {   
        Debug.Log("game controller gold "+ gameController.GetGold()+"PRİCE "+price);
        if (gameController.GetGold() > price && _canPurchase)
        {
            _canPurchase = false;
            EventManager.OnGoldAndExpChanged(-price,0);
            EventManager.OnRelicTaken(GetRandomRelic());
            UpdatePrice();
            StartCoroutine(WaitBetweenPurchases());
        }
        
    }

    private void UpdatePrice()
    {
        
        price =(int)(price * priceIncreaseModifier);
        priceText.text = price.ToString();
    }
    
    public RelicTypes GetRandomRelic()
    {
        List<RelicTypes> relicTypesList = relicScriptableObject.GetRelicTypesList();
        int randomNumber = Random.Range(0, relicTypesList.Count);
        return relicTypesList[randomNumber];
    }

    private IEnumerator WaitBetweenPurchases()
    {
        yield return new WaitForSeconds(0.2f);
        _canPurchase = true;
    }
}
