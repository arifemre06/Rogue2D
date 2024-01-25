using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class RelicPurchaseUITemplate : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image soldImage;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RelicScriptableObject relicScriptableObject;
    private int price;
    [SerializeField] private int basePrice;
    private bool _clicked;
    private RelicTypes _relicTypes;
    
    private ColorBlock _purchaseButtonBaseColors;
    
    private const float priceIncreaseModifier = 1.05f;
    private const float priceIncreaseModifierForLevelUp = 1.2f;
    
    private const float CommonPossibilities = 50;
    private const float RarePossibilities = 40;
    private const float LegendaryPossibilities = 10;

    private const float RareRelicPriceIncreaseMultiplier = 3;
    private const float LegendaryRelicPriceIncreaseMultiplier = 5;

    private void Awake()
    {
        price = basePrice;
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        EventManager.ReRollShop += OnReRollShop;
        EventManager.UpdateShopPrices += UpdatePrice;
        EventManager.NextLevel += OnNextLevel;
    }

    private void OnDestroy()
    {
        EventManager.ReRollShop -= OnReRollShop;
        EventManager.UpdateShopPrices -= UpdatePrice;
        EventManager.NextLevel -= OnNextLevel;
    }

    void Start()
    {   
        _purchaseButtonBaseColors = purchaseButton.colors;
        SetUIWhenStart();
    }
    
    private void OnNextLevel(int obj)
    {
        price = (int)(basePrice * priceIncreaseModifierForLevelUp * obj);
        priceText.text = price.ToString();
        SetUIWhenStart();
        purchaseButton.colors = _purchaseButtonBaseColors;
    }
    private void OnPurchaseButtonClicked()
    {
        if (gameController.GetGold() >= price && !_clicked)
        {
            _clicked = true;
            EventManager.OnGoldAndExpChanged(-price,0);
            EventManager.OnRelicTaken(_relicTypes);
            EventManager.OnUpdateShopPrices();
            UpdateToSoldUI();
        }
    }

    private void UpdatePrice()
    {
        if (!_clicked)
        {
            price =(int)(price * priceIncreaseModifier);
            priceText.text = price.ToString();
        }
    }
    
    private void OnReRollShop()
    {
        _relicTypes = GetRandomRelic();
        itemImage.sprite = relicScriptableObject.GetPrefab(_relicTypes);
        headerText.text = _relicTypes.ToString();
        descriptionText.text = relicScriptableObject.GetRelicText(_relicTypes);
        soldImage.enabled = false;
        price = (int)(basePrice * priceIncreaseModifierForLevelUp * gameController.LevelIndex);
        SetUIWhenStart();
        purchaseButton.colors = _purchaseButtonBaseColors;
        _clicked = false;
    }
    public RelicTypes GetRandomRelic()
    {
        float randomRarity = Random.Range(0,100);
        Debug.Log("kac attık reis "+randomRarity);
        if (randomRarity < LegendaryPossibilities)
        {
            price = (int)(price * LegendaryRelicPriceIncreaseMultiplier);
            priceText.text = price.ToString();
            return GetRandomLegendaryRelic();
            
        }
        else if (randomRarity is > LegendaryPossibilities and < RarePossibilities)
        {
            price = (int)(price * RareRelicPriceIncreaseMultiplier);
            priceText.text = price.ToString();
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

    private void UpdateToSoldUI()
    {
        soldImage.enabled = true;
        ChangeButtonUIToSold();
        CloseOldUI();
    }

    private void ChangeButtonUIToSold()
    {
        var colors = purchaseButton.colors;
        colors.normalColor = Color.black;
        colors.pressedColor = Color.black;
        colors.highlightedColor = Color.black;
        purchaseButton.colors = colors;
        priceText.text = "";
    }

    private void CloseOldUI()
    {
        headerText.text = "";
    }

    private void SetUIWhenStart()
    {
        soldImage.enabled = false;
        _relicTypes = GetRandomRelic();
        itemImage.sprite = relicScriptableObject.GetPrefab(_relicTypes);
        headerText.text = _relicTypes.ToString();
        descriptionText.text = relicScriptableObject.GetRelicText(_relicTypes);
        priceText.text = price.ToString();
    }
}
