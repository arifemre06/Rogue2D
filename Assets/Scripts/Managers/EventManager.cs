using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enemies;
using ScriptableObjectsScripts;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<Sprite,string> RelicCollected; 
    public static event Action<float> AttackSpeedRelicCollected;
    public static event Action<float> AttackDamageRelicCollected;

    public static event Action<RuinTypes> RuinEffectTaken;
    public static event Action RuinGiveMeTrioTaken;
    public static event Action RuinHighRiskHighRewardTaken;

    public static event Action<BaseEnemy> EnemySpawned; 
    public static event Action<BaseEnemy,int, int> EnemyKilled;
    public static event Action<int, int> GoldAndExpChanged;

    public static event Action<GameState, GameState> GameStateChanged;
    public static event Action<bool> InfoPanelOpenOrClose;
    public static event Action MainMenuButtonClicked; 
    public static event Action GameStarted;
    public static event Action<int> UpGradePanelOpened;
    public static event Action Quit;

    public static event Action<float> MusicVolumeChanged;
    public static event Action<float> GameVolumeChanged; 




    public static void OnAttackSpeedRelicCollected(float obj)
    {
        AttackSpeedRelicCollected?.Invoke(obj);
    }

    public static void OnAttackDamageRelicCollected(float obj)
    {
        AttackDamageRelicCollected?.Invoke(obj);
    }

    public static void OnGameStateChanged(GameState arg1, GameState arg2)
    {
        GameStateChanged?.Invoke(arg1, arg2);
    }

    public static void OnEnemyKilled(BaseEnemy enemy,int arg1, int arg2)
    {
        EnemyKilled?.Invoke(enemy,arg1, arg2);
    }

    public static void OnGoldAndExpChanged(int arg1, int arg2)
    {
        GoldAndExpChanged?.Invoke(arg1, arg2);
    }

    public static void OnRelicCollected(Sprite sprite,string text)
    {
        RelicCollected?.Invoke(sprite,text);
    }

    public static void OnQuit()
    {
        Quit?.Invoke();
    }

    public static void OnGameStarted()
    {
        GameStarted?.Invoke();
    }

    public static void OnMusicVolumeChanged(float obj)
    {
        MusicVolumeChanged?.Invoke(obj);
    }

    public static void OnGameVolumeChanged(float obj)
    {
        GameVolumeChanged?.Invoke(obj);
    }

    public static void OnMainMenuButtonClicked()
    {
        MainMenuButtonClicked?.Invoke();
    }

    public static void OnEnemySpawned(BaseEnemy obj)
    {
        EnemySpawned?.Invoke(obj);
    }

    public static void OnInfoPanelOpened(bool isOpen)
    {
        InfoPanelOpenOrClose?.Invoke(isOpen);
    }

    public static void OnUpGradePanelOpened(int wave)
    {
        UpGradePanelOpened?.Invoke(wave);
    }

    public static void OnRuinEffectTaken(RuinTypes obj)
    {
        RuinEffectTaken?.Invoke(obj);
    }

    public static void OnRuinGiveMeTrioTaken()
    {
        RuinGiveMeTrioTaken?.Invoke();
    }

    public static void OnRuinHighRiskHighRewardTaken()
    {
        RuinHighRiskHighRewardTaken?.Invoke();
    }
}
