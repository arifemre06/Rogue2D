using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DefaultNamespace.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class SettingsPanelScript : UIPanel
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Slider gameSoundSlider;
        [SerializeField] private Slider musicSoundSlider;
        [SerializeField] private TextMeshProUGUI gameSoundValueText;
        [SerializeField] private TextMeshProUGUI musicSoundValueText;


        private void Awake()
        {
            mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
            gameSoundSlider.onValueChanged.AddListener(OnGameSoundValueChanged);
            musicSoundSlider.onValueChanged.AddListener(OnMusicSoundValueChanged);
        }

        private void Start()
        {
            float gameSound = PlayerPrefs.GetFloat("GameSoundVolume");
            gameSoundSlider.value = gameSound;
            float musicSound = PlayerPrefs.GetFloat("MusicVolume");
            musicSoundSlider.value = musicSound;
            float musicTextValue =  musicSound  * 100;
            float gameTextValue = gameSound * 100;
            gameSoundValueText.text = gameTextValue.ToString("#.");
            musicSoundValueText.text = musicTextValue.ToString("#.");
        }

        private void OnMusicSoundValueChanged(float value)
        {
            float textValue = value * 100;
            musicSoundValueText.text = "%" + textValue.ToString("#.");
            EventManager.OnMusicVolumeChanged(value);
        }

        private void OnGameSoundValueChanged(float value)
        {
            float textValue = value * 100;
            gameSoundValueText.text = "%" + textValue.ToString("#.");
            EventManager.OnGameVolumeChanged(value);
        }

        private void MainMenuButtonClicked()
        {
            EventManager.OnMainMenuButtonClicked();
        }

    }
}
