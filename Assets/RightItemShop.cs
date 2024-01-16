using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image obtainedRelicImage;
    [SerializeField] private Image chestImage;
    private RelicTypes _obtainedRelic;
    private bool _canPurchase;

    private void Awake()
    {
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
    }

    private void Start()
    {
        obtainedRelicImage.enabled = false;
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
            _obtainedRelic = GetRandomRelic();
            EventManager.OnRelicTaken(_obtainedRelic);
            UpdatePrice();
            StartCoroutine(WaitBetweenPurchases());
            UpdateToSoldUI();
        }
    }

    private void UpdateToSoldUI()
    {
        obtainedRelicImage.enabled = true;
        chestImage.enabled = false;
        obtainedRelicImage.sprite = relicScriptableObject.GetPrefab(_obtainedRelic);
        headerText.text = _obtainedRelic.ToString();
        descriptionText.text = relicScriptableObject.GetRelicText(_obtainedRelic);
        priceText.text = "Obtained";
        _canPurchase = false;
        StartCoroutine(WaitObtainedRelicUI());

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

    private IEnumerator WaitObtainedRelicUI()
    {
        yield return new WaitForSeconds(1);
        SetChestUI();
        _canPurchase = true;
    }

    private void SetChestUI()
    {
        chestImage.enabled = true;
        obtainedRelicImage.enabled = false;
        priceText.text = price.ToString();
        headerText.text = "Chest";
        descriptionText.text = "Get A Random Relic";
    }
}
