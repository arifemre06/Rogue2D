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
        [SerializeField] private TextMeshProUGUI nextLevelText;
        [SerializeField] private TextMeshProUGUI goldText;
        private void Awake()
        {
            nextLevel.onClick.AddListener(OnNextLevelClicked);
            EventManager.GoldAndExpChanged += OnGoldChanged;
            EventManager.UpGradePanelOpened += OnUpgradePanelOpened;
        }

        private void OnDestroy()
        {
            EventManager.GoldAndExpChanged -= OnGoldChanged;
            EventManager.UpGradePanelOpened -= OnUpgradePanelOpened;
        }

        private void OnUpgradePanelOpened(int obj)
        {
            int wave = gameController.LevelIndex;
            nextLevelText.text = "Wave:" + (wave + 1);
        }

        private void Start()
        {
            goldText.text = "Gold:" + gameController.GetGold();
            int wave = gameController.LevelIndex;
            nextLevelText.text = "Wave:" + (wave + 1);
        }

        public override void ActivatePanel()
        {  
            
            EventManager.GoldAndExpChanged += OnGoldChanged;
            goldText.text ="Gold:" + gameController.GetGold();
            base.ActivatePanel();
        }

        public override void DeActivatePanel()
        {
            
            EventManager.GoldAndExpChanged -= OnGoldChanged;
            base.DeActivatePanel();
        }

        private void OnNextLevelClicked()
        {   
            
            EventManager.OnNextLevel(gameController.LevelIndex);
        }
        
        private void OnGoldChanged(int gold, int exp)
        {   
            Debug.Log("ee biz abonelikten cıktık ama");
            StartCoroutine(UpdateGoldText());
        }

        private IEnumerator UpdateGoldText()
        {
            yield return new WaitForSeconds(0.2f);
            goldText.text = "Gold:" + gameController.GetGold();
        }
        
    }
}
