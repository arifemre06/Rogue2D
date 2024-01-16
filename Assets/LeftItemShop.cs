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

    private void Awake()
    {
        reRollButton.onClick.AddListener(OnReRollButtonClicked);
    }

    private void Start()
    {
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
        rerollPrice =(int)(rerollPrice * priceIncreaseModifier);
        priceText.text = "reroll: " + rerollPrice;
    }
}

