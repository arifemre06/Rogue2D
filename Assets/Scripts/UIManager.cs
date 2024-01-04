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
    
    private void Awake()
    {
        EventManager.GameStateChanged += OnGameStateChanged;
        DeActivateAllPanels();
            
    }
    
    private void OnDestroy()
    {
        EventManager.GameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState oldState, GameState newState)
    {   
        Debug.Log("panelleri kapat?");
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
    }
    
    private void DeActivateAllPanels()
    {   
        
        inGamePanel.DeActivatePanel();
        mainMenuPanel.DeActivatePanel();
        settingsPanel.DeActivatePanel();
        
        
    }
    
    private void ActivatePanel(UIPanel panel)
    {
        panel.ActivatePanel();
        
        
    }
    

    
}
