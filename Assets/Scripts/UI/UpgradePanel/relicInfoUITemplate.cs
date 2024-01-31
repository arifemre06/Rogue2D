using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class relicInfoUITemplate : MonoBehaviour
{
    [SerializeField] private Image relicImage;
    [SerializeField] private TextMeshProUGUI relicAmount;

    public void SetAmountAndSprite(Sprite sprite,string amount)
    {
        relicImage.sprite = sprite;
        relicAmount.text = amount;
    }
}
