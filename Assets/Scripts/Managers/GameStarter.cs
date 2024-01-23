using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameStarter : MonoBehaviour
    {

        private void Awake()
        {
            EventManager.PreGameStarted += OnPreGameStarted;
        }
        private void OnDestroy()
        {
            EventManager.PreGameStarted += OnPreGameStarted;
        }

        private void OnPreGameStarted()
        {
            ResetEveryThingForNewGame();
            EventManager.OnGameStarted();
        }

        private void ResetEveryThingForNewGame()
        {   
            TakenRelics.ClearTakenRelics();
            TakenRuins.ClearTakenRuins();
            /*
            foreach (BaseHero baseHero in heroesInScene)
            {
                Instantiate(baseHero, Vector3.zero, quaternion.identity);
            }
            */
            
        }
    }
}