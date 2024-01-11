using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enemies;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class gameController : MonoBehaviour
{
    [SerializeField] private List<BaseEnemy> enemyPrefabs;
    public int gold = 0;
    public int exp = 0;

    public static GameState gameState{get; private set;}

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

    //Used to put active enemies in a list
    private static List<BaseEnemy> activeEnemies;
    private int spawnedEnemyCount;

    [SerializeField] private Transform player; // player transform
    [SerializeField] private float minDistance; // minimum distance from spawnpoint to object

    [SerializeField] private List<EnemySpawnData> enemySpawnDataList;
    private float _rangeModifier = 0.4f;
    private int _spawnCounter;
    private int _spawnIndex;
    private int _enemyCounter;
    private int _enemyGroupIndex = 0;
    private int _enemyCountForThatWave = 0;
    private int _waveIndex;
    private List<Vector3> _enemySpawnTransforms;
    private List<EnemyType> _enemyToSpawn;
    private List<float> _secondsBetweenSpawn;
    private List<int> _enemyCountForThatTransform;
    
    
    void Start()
    {
        _waveIndex = 0;
        _spawnIndex = 0;
        _spawnCounter = 0;
        spawnedEnemyCount = 0;
        _enemyCounter = 0;
        _enemyGroupIndex = 0;
        _enemyCountForThatWave = 0;
        ChangeEnemySpawnData(_waveIndex);
        OnMainMenuButtonClicked();
        gold = 0;
        exp = 0;
        
        // if the player has not been set in the inspector log it
        if (player == null)
        {
            Debug.LogWarning("EnemySpawn.Start(): There is no player set in the inspector. Please set this.");
        }

        // if the player has been set in the inspector but the Tag is not set to "Player" log it
        if ((player != null) && (!player.CompareTag("Player")))
        {
            Debug.LogWarning("EnemySpawn.Start(): The Player does not have its Tag set to 'Player'.");
        }

        // if the player has not been set in the inspector and can find by Tag set it
        if ((player == null) && (GameObject.FindGameObjectsWithTag("Player") != null))
        {
            Debug.LogWarning("EnemySpawn.Start(): Setting the missing player.");
            player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        }

        // Starts a coroutine function for spawning bouncing enemies and fly by enemy
        Debug.Log("kac ki bu deger "+_secondsBetweenSpawn[_spawnIndex]);
        
    }
    
    
    void Awake()
        {   
            gameState = GameState.None;
            // Create the list
            activeEnemies = new List<BaseEnemy>();

            _enemySpawnTransforms = new List<Vector3>();
            _enemyToSpawn = new List<EnemyType>();
            _secondsBetweenSpawn = new List<float>();
            _enemyCountForThatTransform = new List<int>();
            
            EventManager.EnemyKilled += SubsriceOnEnemyKilled;
            EventManager.GameStarted += OnGameStart;
            EventManager.Quit += OnQuit;
            EventManager.MainMenuButtonClicked += OnMainMenuButtonClicked;
            EventManager.UpGradePanelOpened += OnUpgradePanelOpened;

        }
    

    private void OnDestroy()
    {
        EventManager.EnemyKilled -= SubsriceOnEnemyKilled;
        EventManager.GameStarted -= OnGameStart;
        EventManager.Quit -= OnQuit;
        EventManager.MainMenuButtonClicked -= OnMainMenuButtonClicked;
        EventManager.UpGradePanelOpened -= OnUpgradePanelOpened;
    }
    
    private void OnGameStart()
    {
        ChangeGameState(GameState.InGamePanel);
        StartCoroutine(SpawnEnemy());
        Time.timeScale = 1;
    }
    
    private void OnMainMenuButtonClicked()
    {
        ChangeGameState(GameState.MainMenu);
        Time.timeScale = 0;
    }
    
    private void OnUpgradePanelOpened(int wave)
    {
        ChangeEnemySpawnData(wave);
        ChangeGameState(GameState.UpgradePanel);
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
    
        IEnumerator SpawnEnemy()
        {   
            
            yield return new WaitForSeconds(_secondsBetweenSpawn[_spawnIndex]);
            _spawnIndex += 1;
            
                //Calls the function to set random position
                Vector3 spawnPoint = PossibleSpawnPointsForThatSpawn();
                
                // Spawn enemy

                if(GetEnemyPrefabFromType(_enemyToSpawn[_enemyCounter]) != null)
                {
                    
                    for (var i = 0; i < _enemyCountForThatTransform[_enemyGroupIndex]; i++)
                    {   
                        yield return new WaitForSeconds(0.2f);
                        BaseEnemy newEnemy = (BaseEnemy)Instantiate(GetEnemyPrefabFromType(_enemyToSpawn[_enemyCounter]), spawnPoint, Quaternion.identity);
                        spawnedEnemyCount += 1;
                        EventManager.OnEnemySpawned(newEnemy);
                        activeEnemies.Add(newEnemy);
                        _enemyCounter += 1;
                    }
                    _enemyGroupIndex += 1;
                    if (_enemyCountForThatTransform[_enemyGroupIndex] != 0)
                    {
                        StartCoroutine(SpawnEnemy());
                    }
                }
        }
        
        private void ChangeEnemySpawnData(int index)
        {
            _spawnIndex = 0;
            _spawnCounter = 0;
            spawnedEnemyCount = 0;
            _enemyCounter = 0;
            _enemyGroupIndex = 0;
            _enemyCountForThatWave = 0;
            _enemySpawnTransforms.Clear();
            _enemyToSpawn.Clear();
            _secondsBetweenSpawn.Clear();
            _enemyCountForThatTransform.Clear();
            
             List<Vector3> tempEnemySpawnTransforms;
             List<EnemyType> tempEnemyToSpawn;
             List<float> tempSecondsBetweenSpawn;
             List<int> tempEnemyCountForThatTransform;
            
            tempEnemySpawnTransforms = enemySpawnDataList[index].GetEnemySpawnTransforms();
            tempEnemyToSpawn = enemySpawnDataList[index].GetEnemyToSpawn();
            tempSecondsBetweenSpawn = enemySpawnDataList[index].GetSecondsToWaitBetweenSpawn();
            tempEnemyCountForThatTransform = enemySpawnDataList[index].GetEnemyCountForThatTransform();
            
            for (var i = 0; i < tempEnemySpawnTransforms.Count; i++)
            {
                _enemySpawnTransforms.Add(tempEnemySpawnTransforms[i]);
            }
            for (var i = 0; i < tempEnemyToSpawn.Count; i++)
            {
                _enemyToSpawn.Add(tempEnemyToSpawn[i]);
            }
            for (var i = 0; i < tempSecondsBetweenSpawn.Count; i++)
            {
                _secondsBetweenSpawn.Add(tempSecondsBetweenSpawn[i]);
            }
            for (var i = 0; i < tempEnemyCountForThatTransform.Count; i++)
            {
                _enemyCountForThatTransform.Add(tempEnemyCountForThatTransform[i]);
            }
            
            for (var i = 0; i < _enemyCountForThatTransform.Count; i++)
            {
                _enemyCountForThatWave += _enemyCountForThatTransform[i];
            }
            
        }

        private BaseEnemy GetEnemyPrefabFromType(EnemyType enemyType)
        {
            switch (enemyType)
            {
             case   EnemyType.Ogre:
                 return enemyPrefabs[0];
             break;
             case EnemyType.Golem:
                 return enemyPrefabs[1];
             break;
            }

            return null;
        }
            
        private Vector3 PossibleSpawnPointsForThatSpawn()
        {
            bool done = false;
            Vector3 randomPosition = new Vector3();
            Vector3 centerPosition = _enemySpawnTransforms[_spawnCounter];
            float spawnRangeAccordingToSpawnCount = _enemyCountForThatTransform[_spawnCounter] * _rangeModifier;
            _spawnCounter += 1;
            
            while (!done)
            {
                randomPosition.x = (int)UnityEngine.Random.Range(centerPosition.x - spawnRangeAccordingToSpawnCount,
                    centerPosition.x + spawnRangeAccordingToSpawnCount);
                randomPosition.y = (int)UnityEngine.Random.Range(centerPosition.y - spawnRangeAccordingToSpawnCount,
                    centerPosition.y + spawnRangeAccordingToSpawnCount);
                randomPosition.z = 0;
                
                done = ((minDistance == 0) || ValidMinimumDistance(randomPosition));
            }
            
            return randomPosition;
        }

        bool ValidMinimumDistance(Vector3 enemyPosition)
        {
            bool isValid = true;
                minDistance = Mathf.Abs(minDistance);
            float dist;
 
            if (player != null)
            {
                dist = Mathf.Abs(Vector3.Distance(player.position, enemyPosition));
                isValid = (dist > minDistance);
               
            }
 
            if (isValid && (activeEnemies.Count > 0))
            {
                foreach (var t in activeEnemies)
                {
                    if (t != null)
                    {
                        dist = Mathf.Abs(Vector3.Distance(t.transform.position, enemyPosition));
                        //Debug.Log("activeEnemies[" + i + "].position = " + activeEnemies[i].transform.position + ", spawned position = " + enemyPosition + ", calculated distance = " + dist + ", isValid = " + (isValid ? "True" : False"));
                        if (dist < minDistance)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }
           
            return isValid;
        }

        private int GetEnemyCount()
        {
            //return GameObject.FindGameObjectsWithTag("Enemy").Length;
            return spawnedEnemyCount - activeEnemies.Count;
        }
        
        private void SubsriceOnEnemyKilled(BaseEnemy enemy,int gainedGold,int gainedExp)
        {
            gold += gainedGold;
            exp += gainedExp;
            activeEnemies.Remove(enemy);
            EventManager.OnGoldAndExpChanged(gold,exp);
            int killedEnemyCount = GetEnemyCount();
            int aliveEnemyCount = (spawnedEnemyCount - killedEnemyCount);
            Debug.Log("alive enemy count "+aliveEnemyCount + " spawned enemy count "+ spawnedEnemyCount + " killed enemy count "+killedEnemyCount);
            if (killedEnemyCount >= _enemyCountForThatWave)
            {
                Debug.Log("level is over wave index "+_waveIndex);
                _waveIndex += 1;
                if (_waveIndex < enemySpawnDataList.Count)
                {
                    EventManager.OnUpGradePanelOpened(_waveIndex);
                }
                
            }
        }
        
        
        
        

        
    }

public enum GameState
{
    None,
    InGamePanel,
    MainMenu,
    Settings,
    UpgradePanel
}

public enum EnemyType
{
    Ogre,
    Golem
}

        