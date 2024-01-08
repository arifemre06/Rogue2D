using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class gameController : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
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
    private static List<GameObject> activeEnemies;
    private int spawnedEnemyCount;

    [SerializeField] private Transform player; // player transform
    [SerializeField] private float minDistance; // minimum distance from spawnpoint to object
    
    
    void Start()
    {
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
        StartCoroutine(SpawnEnemy());
    }

    

    void Awake()
        {   
            gameState = GameState.None;
            // Create the list
            activeEnemies = new List<GameObject>();
            EventManager.EnemyKilled += SubsriceOnEnemyKilled;
            EventManager.GameStarted += OnGameStart;
            EventManager.Quit += OnQuit;
            EventManager.MainMenuButtonClicked += OnMainMenuButtonClicked;
            
        }

    private void OnDestroy()
    {
        EventManager.EnemyKilled -= SubsriceOnEnemyKilled;
        EventManager.GameStarted -= OnGameStart;
        EventManager.Quit -= OnQuit;
        EventManager.MainMenuButtonClicked -= OnMainMenuButtonClicked;
    }
    
    private void OnGameStart()
    {
        ChangeGameState(GameState.InGamePanel);
        Time.timeScale = 1;
        
    }
    
    private void OnMainMenuButtonClicked()
    {
        ChangeGameState(GameState.MainMenu);
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
    // Spawn a Bouncing enemy
        IEnumerator SpawnEnemy()
        {
            //Wait 3 to 5 seconds when game starts to spawn a ball
            yield return new WaitForSeconds(Random.Range(1, 3));

            while (true)
            {
                //Calls the function to set random position
                
                Vector3 spawnPoint = RandomPointWithinBorders();
                
                // Spawn enemy
                spawnedEnemyCount += 1;
                int aliveEnemyCount = GetEnemyCount();
                int killedEnemyCount = (spawnedEnemyCount - aliveEnemyCount);
                //Debug.Log("alive enemy count "+aliveEnemyCount + " spawned enemy count "+ spawnedEnemyCount + " killed enemy count "+killedEnemyCount);
                int randomNumber = Random.Range(0, enemyPrefabs.Count);
                GameObject newEnemy = (GameObject)Instantiate(enemyPrefabs[randomNumber], spawnPoint, Quaternion.identity);
                EventManager.OnEnemySpawned(newEnemy);
                
                activeEnemies.Add(newEnemy);
                Ogre ogre = newEnemy.GetComponent<Ogre>();
                yield return new WaitForSeconds(Random.Range(2, 4));
            }
        }


        Vector3 RandomPointWithinBorders()
        {
            bool done = false;
            
            Vector3 randomPosition = new Vector3();

            while (!done)
            {
                //Code that will spawn a bouncing ball and ShowSpawn at a random position inside the borders 
                var position = sceneborderLeft.position;
                var position1 = borderRight.position;
                randomPosition.x = (int)UnityEngine.Random.Range(Math.Max(position.x,borderLeft.position.x), Math.Min(position1.x,position.x));
                randomPosition.y = (int)UnityEngine.Random.Range(Math.Max(sceneborderBottom.position.y,borderBottom.position.y), Math.Min(sceneborderTop.position.y,borderTop.position.y));
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

        int GetEnemyCount()
        {
            return GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        
        private void SubsriceOnEnemyKilled(GameObject enemy,int gainedGold,int gainedExp)
        {
            gold += gainedGold;
            exp += gainedExp;
            EventManager.OnGoldAndExpChanged(gold,exp);
            //Debug.Log("bana ulastı gold: "+gold +"exp: "+exp);
        }
        
        
        
        

        
    }

public enum GameState
{
    None,
    InGamePanel,
    MainMenu,
    Settings
}

        