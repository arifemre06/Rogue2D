using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Enemies;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class gameController : MonoBehaviour
{
    public static GameState gameState{get; private set;}
    private static int Gold;
    public static int LevelIndex{get; private set;}
    public static int HeroCountOnScene;
    
    public int exp = 0;

    // Borders
    public Transform borderTop;
    public  Transform borderBottom;
    public  Transform borderLeft;
    public  Transform borderRight;
    
    // Game Scene Borders
    public Transform sceneborderTop;
    public  Transform sceneborderBottom;
    public  Transform sceneborderLeft;
    public  Transform sceneborderRight;

    
    
    
    void Start()
    {
        LevelIndex = 0;
        EventManager.OnMainMenuButtonClicked();
        Gold = 0;
        exp = 0;
    }
    void Awake()
        {   
            gameState = GameState.None;
            EventManager.GameStarted += OnGameStart;
            EventManager.Quit += OnQuit;
            EventManager.MainMenuButtonClicked += OnMainMenuButtonClicked;
            EventManager.UpGradePanelOpened += OnUpgradePanelOpened;
            EventManager.GoldAndExpChanged += OnGoldAndExpChanged;
            EventManager.GameOver += OnGameOver;
            EventManager.NextLevel += OnNextLevel;
        }
    
    private void OnDestroy()
    {
        EventManager.GameStarted -= OnGameStart;
        EventManager.Quit -= OnQuit;
        EventManager.MainMenuButtonClicked -= OnMainMenuButtonClicked;
        EventManager.UpGradePanelOpened -= OnUpgradePanelOpened;
        EventManager.GoldAndExpChanged -= OnGoldAndExpChanged;
        EventManager.GameOver -= OnGameOver;
        EventManager.NextLevel -= OnNextLevel;
    }

    private void OnGameStart()
    {
        ChangeGameState(GameState.InGamePanel);
        Time.timeScale = 1;
    }
    
    private void OnMainMenuButtonClicked()
    {
        ChangeGameState(GameState.MainMenu);
        //Time.timeScale = 0;
    }
    
    private void OnUpgradePanelOpened(int wave)
    {   
        LevelIndex += 1;
        ChangeGameState(GameState.UpgradePanel);
    }
    
    private void OnNextLevel(int obj)
    {
        ChangeGameState(GameState.InGamePanel);
    }
    
    private void OnGameOver()
    {
        Debug.Log("Game is Over");
        ChangeGameState(GameState.GameOver);
        LevelIndex = 0;
        Gold = 0;
        EventManager.OnGoldAndExpChanged(0,0);
        Time.timeScale = 0;
    }
    private void OnQuit()
    {
        Application.Quit();
    }
    
    private void ChangeGameState(GameState newState)
    {
        var oldState = gameState;
        Debug.Log($"changing to state {oldState} - {newState}");
        gameState = newState;
        EventManager.OnGameStateChanged(oldState, newState);
        
    }
    
        //enemy manageri burdan söktük
        
        private void OnGoldAndExpChanged(int arg1, int arg2)
        {
            Gold += arg1;
            exp += arg2;
        }

        public static int GetGold()
        {
            return Gold;
        }
}

public enum GameState
{
    None,
    InGamePanel,
    MainMenu,
    GameOver,
    Settings,
    UpgradePanel
}

public enum EnemyType
{
    Ogre,
    Golem,
    Wraith
}

        