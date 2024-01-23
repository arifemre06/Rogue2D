using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{   
    
    [SerializeField] private UIPanel inGamePanel;
    [SerializeField] private UIPanel mainMenuPanel;
    [SerializeField] private UIPanel settingsPanel;
    [SerializeField] private UIPanel infoPanel;
    [SerializeField] private UIPanel upgradePanel;
    [SerializeField] private UIPanel gameOverPanel;

    private InfoPanel _infoPanelScript;
    private bool _infoPanelOpened;
    
    private void Awake()
    {
        EventManager.GameStateChanged += OnGameStateChanged;
        EventManager.InfoPanelOpenOrClose += OnInfoPanelStatusChanged;
        _infoPanelScript = infoPanel.GetComponent<InfoPanel>();
        DeActivateAllPanels();
            
    }
    
    private void OnDestroy()
    {
        EventManager.GameStateChanged -= OnGameStateChanged;
        EventManager.InfoPanelOpenOrClose -= OnInfoPanelStatusChanged;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K) && !_infoPanelOpened)
        {
            OnInfoPanelStatusChanged(true);
            _infoPanelScript.UpdateInfoPanel();
            _infoPanelOpened = true;
        }
        else if (!Input.GetKey(KeyCode.K) && _infoPanelOpened)
        {
            OnInfoPanelStatusChanged(false);
            _infoPanelOpened = false;
        }
    }

    private void OnGameStateChanged(GameState oldState, GameState newState)
    {
        DeActivateAllPanels();
        if (newState == GameState.InGamePanel)
        {
            ActivatePanel(inGamePanel);
        }
        else if (newState == GameState.Settings)
        {
            ActivatePanel(settingsPanel);
        }
        else if (newState == GameState.MainMenu)
        {
            ActivatePanel(mainMenuPanel);
        }
        else if (newState == GameState.UpgradePanel)
        {
            ActivatePanel(upgradePanel);
        }
        else if (newState == GameState.GameOver)
        {
            ActivatePanel(gameOverPanel);
        }
    }
    
    private void DeActivateAllPanels()
    {   
        
        //inGamePanel.DeActivatePanel();
        mainMenuPanel.DeActivatePanel();
        settingsPanel.DeActivatePanel();
        infoPanel.DeActivatePanel();
        upgradePanel.DeActivatePanel();
        gameOverPanel.DeActivatePanel();
        
    }
    
    private void ActivatePanel(UIPanel panel)
    {
        panel.ActivatePanel();

    }
    
    private void OnInfoPanelStatusChanged(bool isOpen)
    {
        if (isOpen)
        {
            infoPanel.ActivatePanel();
        }
        else
        {
            infoPanel.DeActivatePanel();
        }
       
    }
}
