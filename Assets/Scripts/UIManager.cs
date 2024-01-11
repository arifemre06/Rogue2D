using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{   
    
    [SerializeField] private UIPanel inGamePanel;
    [SerializeField] private UIPanel mainMenuPanel;
    [SerializeField] private UIPanel settingsPanel;
    [SerializeField] private UIPanel infoPanel;
    [SerializeField] private UIPanel upgradePanel;
    
    private void Awake()
    {
        EventManager.GameStateChanged += OnGameStateChanged;
        EventManager.InfoPanelOpenOrClose += OnInfoPanelStatusChanged;
        DeActivateAllPanels();
            
    }
    
    private void OnDestroy()
    {
        EventManager.GameStateChanged -= OnGameStateChanged;
        EventManager.InfoPanelOpenOrClose -= OnInfoPanelStatusChanged;
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
    }
    
    private void DeActivateAllPanels()
    {   
        
        inGamePanel.DeActivatePanel();
        mainMenuPanel.DeActivatePanel();
        settingsPanel.DeActivatePanel();
        infoPanel.DeActivatePanel();
        upgradePanel.DeActivatePanel();
        
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
