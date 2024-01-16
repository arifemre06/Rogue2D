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
    [SerializeField] private int price;
    [SerializeField] private float priceIncreaseModifier;
    private bool _clicked;
    private RelicTypes _relicTypes;

    private void Awake()
    {
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        EventManager.ReRollShop += OnReRollShop;
        EventManager.UpdateShopPrices += UpdatePrice;
    }

    private void OnDestroy()
    {
        EventManager.ReRollShop -= OnReRollShop;
        EventManager.UpdateShopPrices -= UpdatePrice;
    }

    void Start()
    {
        SetUIWhenStart();
    }
    private void OnPurchaseButtonClicked()
    {
        if (gameController.GetGold() > price && !_clicked)
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
        if (!_clicked)
        {
            _relicTypes = GetRandomRelic();
            itemImage.sprite = relicScriptableObject.GetPrefab(_relicTypes);
            headerText.text = _relicTypes.ToString();
            descriptionText.text = relicScriptableObject.GetRelicText(_relicTypes);
        }
    }
    private RelicTypes GetRandomRelic()
    {
        List<RelicTypes> relicTypesList = relicScriptableObject.GetRelicTypesList();
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
