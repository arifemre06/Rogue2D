using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : UIPanel
{
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        EventManager.OnMainMenuButtonClicked();
    }
}
