using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using ScriptableObjectsScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class InGamePanel : UIPanel
    {   
        //gold and exp ui
        [SerializeField] private TextMeshProUGUI goldText;
        
        //Relic info ui
        [SerializeField] private TextMeshProUGUI relicText;
        [SerializeField] private Image relicSprite;
    
        private void Awake()
        {
            EventManager.GoldAndExpChanged += OnGoldChanged;
            EventManager.RelicCollected += OnRelicCollected;
            relicSprite.enabled = false;
            relicText.enabled = false;
        }

        private void OnDestroy()
        {
            EventManager.GoldAndExpChanged -= OnGoldChanged;
            EventManager.RelicCollected -= OnRelicCollected;
        }
        private void OnRelicCollected(Sprite sprite,string text)
        {   
            relicSprite.sprite = sprite;
            relicText.text = text;
            relicSprite.enabled = true;
            relicText.enabled = true;
            StartCoroutine(HideRelicInfo());
        }

        IEnumerator HideRelicInfo()
        {
            yield return new WaitForSeconds(3);
            relicSprite.enabled = false;
            relicText.enabled = false;
        }
        
        private void OnGoldChanged(int gold, int exp)
        {
            StartCoroutine(UpdateGoldText());
        }

        private IEnumerator UpdateGoldText()
        {
            yield return new WaitForSeconds(0.5f);
            goldText.text = gameController.GetGold().ToString();
        }
    }
}
