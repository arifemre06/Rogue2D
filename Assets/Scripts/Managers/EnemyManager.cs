using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enemies;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{   
    //Used to put active enemies in a list
    private static List<BaseEnemy> activeEnemies;
    private int spawnedEnemyCount;

    [SerializeField] private RuinStatue ruinStatue;
    private RuinStatue _ruinToInstantiate;
    [SerializeField] private Transform player; // player transform
     
    [SerializeField] private List<BaseEnemy> enemyPrefabs;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private int totalEnemyPowerForFirstLevel;
    
    private int _remainingEnemyPowerToSpawn;
    private int _totalEnemyPowerForThatLevel;
    private int _spawnIndex;
    private bool _spawnFinished;

    private const int WaitSecondsFirstSpawn = 4;
    // minimum distance from spawnpoint to object
    private const float MinDistance = 2;
    private const float SpawnDistanceToPlayer = 5;
    private const float GroupSpawnRange = 2;
    private const int MaxEnemyCountForOneSpawn = 2;
    private const int MinEnemyCountForOneSpawn = 1;
    private const float MaxWaitTimeBetweenSpawns = 0.4f;
    private const float MinWaitTimeBetweenSpawns = 0.2f;
    private const float PowerIncreaseModifier = 1.2f;

    private void Awake()
    {
        // Create the list
        activeEnemies = new List<BaseEnemy>();

        EventManager.GameStarted += OnGameStart;
        EventManager.NextLevel += OnNextLevel;
        EventManager.EnemyKilled += SubsriceOnEnemyKilled;
        EventManager.GameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        EventManager.GameStarted -= OnGameStart;
        EventManager.NextLevel -= OnNextLevel;
        EventManager.EnemyKilled -= SubsriceOnEnemyKilled;
        EventManager.GameOver -= OnGameOver;
    }
    
    private void OnGameStart()
    {
        StartCoroutine(SpawnEnemiesForThatLevel());
    }
    
    private void OnNextLevel(int level)
    {
        ChangeEnemySpawnData(level);
        InstantiateRuins(level);
        StartCoroutine(SpawnEnemiesForThatLevel());
    }
    
    private void OnGameOver()
    {
        ResetEverythingForAnewGame();
    }

    private IEnumerator SpawnEnemiesForThatLevel()
    {
        yield return new WaitForSeconds(WaitSecondsFirstSpawn);
        StartCoroutine(SpawnEnemy());
    }

    private void Start()
    {
        //gameController.LevelIndex = 0;
        spawnedEnemyCount = 0;
        ChangeEnemySpawnData(gameController.LevelIndex);
        
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
    }


    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds((int)UnityEngine.Random.Range(MinWaitTimeBetweenSpawns,MaxWaitTimeBetweenSpawns));

        //Calls the function to set random position
                Vector3 spawnPoint = PossibleSpawnPointsForThatSpawn();
                
                // Spawn enemy

                if(_remainingEnemyPowerToSpawn > 0)
                {
                    int enemyCountForThatSpawn = (int)UnityEngine.Random.Range(MinEnemyCountForOneSpawn, MaxEnemyCountForOneSpawn);
                    for (var i = 0; i < enemyCountForThatSpawn; i++)
                    {   
                        
                        int whichEnemyWillSpawn = (int)UnityEngine.Random.Range(0, enemyPrefabs.Count);
                        Vector2 spawnPosition =
                            new Vector2(
                                UnityEngine.Random.Range(spawnPoint.x - GroupSpawnRange,
                                    spawnPoint.x + GroupSpawnRange),
                                UnityEngine.Random.Range(spawnPoint.y - GroupSpawnRange,
                                    spawnPoint.y + GroupSpawnRange));

                        GameObject warningSprite = Instantiate(warningPrefab, spawnPosition, Quaternion.identity);
                        yield return new WaitForSeconds(0.3f);
                        Destroy(warningSprite);
                        
                        BaseEnemy newEnemy = (BaseEnemy)Instantiate(enemyPrefabs[whichEnemyWillSpawn],spawnPosition , Quaternion.identity);
                        _remainingEnemyPowerToSpawn -= newEnemy.GetPower();
                        spawnedEnemyCount += 1;
                        EventManager.OnEnemySpawned(newEnemy);
                        activeEnemies.Add(newEnemy);
                        if (_remainingEnemyPowerToSpawn < 0)
                        {
                            break;
                        }
                    }

                    StartCoroutine(SpawnEnemy());
                }
                else
                {
                    _spawnFinished = true;
                }
    }
    
    private void ChangeEnemySpawnData(int index)
        {
            
            spawnedEnemyCount = 0;
            _spawnFinished = false;
            if (index != 0)
            {
                _totalEnemyPowerForThatLevel =(int)(totalEnemyPowerForFirstLevel * index * PowerIncreaseModifier);
            }
            else
            {
                _totalEnemyPowerForThatLevel = totalEnemyPowerForFirstLevel;
            }

            _remainingEnemyPowerToSpawn = _totalEnemyPowerForThatLevel;
        }

        private Vector3 PossibleSpawnPointsForThatSpawn()
        {
            bool done = false;
            Vector3 randomPosition = new Vector3();
            Vector3 centerPosition = player.transform.position;

            while (!done)
            {
                randomPosition.x = (int)UnityEngine.Random.Range(centerPosition.x - SpawnDistanceToPlayer,
                    centerPosition.x + SpawnDistanceToPlayer);
                randomPosition.y = (int)UnityEngine.Random.Range(centerPosition.y - SpawnDistanceToPlayer,
                    centerPosition.y + SpawnDistanceToPlayer);
                randomPosition.z = 0;
                
                done = ((MinDistance == 0) || ValidMinimumDistance(randomPosition));
            }
            return randomPosition;
        }

        bool ValidMinimumDistance(Vector3 enemyPosition)
        {
            bool isValid = true;
                
            float dist;
 
            if (player != null)
            {
                dist = Mathf.Abs(Vector3.Distance(player.position, enemyPosition));
                isValid = (dist > MinDistance);
            }
 
            if (isValid && (activeEnemies.Count > 0))
            {
                foreach (var t in activeEnemies)
                {
                    if (t != null)
                    {
                        dist = Mathf.Abs(Vector3.Distance(t.transform.position, enemyPosition));
                        //Debug.Log("activeEnemies[" + i + "].position = " + activeEnemies[i].transform.position + ", spawned position = " + enemyPosition + ", calculated distance = " + dist + ", isValid = " + (isValid ? "True" : False"));
                        if (dist < MinDistance)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }
            return isValid;
        }
        
        private void SubsriceOnEnemyKilled(BaseEnemy enemy,int gainedGold,int gainedExp)
        {
            activeEnemies.Remove(enemy);
            EventManager.OnGoldAndExpChanged(gainedGold,gainedExp);
            int killedEnemyCount = GetEnemyCount();
            int aliveEnemyCount = (spawnedEnemyCount - killedEnemyCount);
            //Debug.Log("alive enemy count "+aliveEnemyCount + " spawned enemy count "+ spawnedEnemyCount + " killed enemy count "+killedEnemyCount);
            if (aliveEnemyCount == 0 && _spawnFinished)
            {
                EventManager.OnUpGradePanelOpened(gameController.LevelIndex);
            }
        }
        private int GetEnemyCount()
        {
            //return GameObject.FindGameObjectsWithTag("Enemy").Length;
            return spawnedEnemyCount - activeEnemies.Count;
        }

        private void ResetEverythingForAnewGame()
        {
            ChangeEnemySpawnData(0);
            foreach (BaseEnemy activeEnemy in activeEnemies)
            {
                Destroy(activeEnemy.gameObject);
            }
            activeEnemies.Clear();
        }

        private void InstantiateRuins(int level)
        {
            if (level % 5 == 0)
            {
                _ruinToInstantiate = Instantiate(ruinStatue, new Vector2(1, 2), quaternion.identity);
            }
            else if (level % 5 != 0 && _ruinToInstantiate != null)
            {
                Destroy(_ruinToInstantiate.gameObject);
            }
            
        }
}
