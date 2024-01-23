using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeftItemShop : MonoBehaviour
{
    [SerializeField] private Button reRollButton;
    [SerializeField] private int rerollPrice;
    [SerializeField] private float priceIncreaseModifier;
    [SerializeField] private TextMeshProUGUI priceText;
    private int _baseRerollPrice;
    private const float priceIncreaseModifierForLevelUp = 1.3f;

    private void Awake()
    {
        _baseRerollPrice = rerollPrice;
        reRollButton.onClick.AddListener(OnReRollButtonClicked);
        EventManager.NextLevel += OnNextLevel;
    }

    private void OnDestroy()
    {
        EventManager.NextLevel -= OnNextLevel;
    }

    private void Start()
    {
        priceText.text = "reroll: " + rerollPrice;
    }
    
    private void OnNextLevel(int obj)
    {
        rerollPrice = (int)(_baseRerollPrice * priceIncreaseModifierForLevelUp * obj);
        priceText.text = "reroll: " + rerollPrice;
    }
    
    private void OnReRollButtonClicked()
    {
        if(gameController.GetGold() > rerollPrice)
        {
        EventManager.OnReRollShop();
        UpdateRerollPrice();
        }
    }

    private void UpdateRerollPrice()
    {
        EventManager.OnGoldAndExpChanged(-rerollPrice,0);
        rerollPrice =(int)(_baseRerollPrice * priceIncreaseModifier);
        priceText.text = "reroll: " + rerollPrice;
    }
}

