using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class MainMenuPanelScript : UIPanel
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        //[SerializeField] private Button continueButton;
        //[SerializeField] private Button tutorialButton;
        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsClicked);
            //continueButton.onClick.AddListener(OnContinueClicked);
            //tutorialButton.onClick.AddListener(OnTutorialClicked);
        }
            /*
        private void OnTutorialClicked()
        {
            EventManager.RaiseTutorialOpened();
        }

        private void OnContinueClicked()
        {
            EventManager.RaiseContinueButtonClicked();
        }
            */
        private void OnSettingsClicked()
        {
            EventManager.OnGameStateChanged(GameState.MainMenu,GameState.Settings);
        }

        private void OnQuitButtonClicked()
        {
            EventManager.OnQuit();
        }

        private void OnStartButtonClicked()
        {
            EventManager.OnGameStarted();
        }
    }
}