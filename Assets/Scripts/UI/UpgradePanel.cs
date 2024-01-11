using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace DefaultNamespace
{
    public class UpgradePanel : UIPanel
    {
        [SerializeField] private Button nextLevel;
        void Awake()
        {
            nextLevel.onClick.AddListener(OnNextLevelClicked);
        }

        private void OnNextLevelClicked()
        {
            EventManager.OnGameStarted();
        }
        
    }
}
