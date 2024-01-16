using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace DefaultNamespace
{
    public class UpgradePanel : UIPanel
    {
        [SerializeField] private Button nextLevel;
        [SerializeField] private TextMeshProUGUI goldText;
        private void Awake()
        {
            nextLevel.onClick.AddListener(OnNextLevelClicked);
            EventManager.GoldAndExpChanged += OnGoldChanged;
        }

        private void OnDestroy()
        {
            EventManager.GoldAndExpChanged -= OnGoldChanged;
        }

        private void Start()
        {
            goldText.text = "Gold:" + gameController.GetGold();
        }

        private void OnNextLevelClicked()
        {
            EventManager.OnGameStarted();
        }
        
        private void OnGoldChanged(int gold, int exp)
        {
            StartCoroutine(UpdateGoldText());
            Debug.Log("gold ne suan "+gold);
        }

        private IEnumerator UpdateGoldText()
        {
            yield return new WaitForSeconds(0.2f);
            goldText.text = "Gold:" + gameController.GetGold();
        }
        
    }
}
