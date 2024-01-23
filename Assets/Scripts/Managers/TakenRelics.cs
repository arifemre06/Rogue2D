using System;
using System.Collections.Generic;
using ScriptableObjectsScripts;
using UnityEngine;

namespace DefaultNamespace
{
    public class TakenRelics : MonoBehaviour
    {
        public static List<RelicTypes> TakenRelicsList { get; private set;}
        private void Awake()
        {
            EventManager.RelicTaken += OnRelicTaken;
            TakenRelicsList = new List<RelicTypes>();
            
        }

        private void OnDestroy()
        {
            EventManager.RelicTaken -= OnRelicTaken;
            
        }

        private void OnRelicTaken(RelicTypes obj)
        {
            TakenRelicsList.Add(obj);
        }

        public static void ClearTakenRelics()
        {
            TakenRelicsList.Clear();
        }
    }
}