using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "EnemySpawn", menuName = "Game/EnemySpawn", order = 0)]
    public class EnemySpawnData : ScriptableObject
    {
        [SerializeField] private List<Vector3> enemySpawnTransforms;
        [SerializeField] private List<EnemyType> enemyToSpawn;
        [SerializeField] private List<float> secondsBetweenSpawn;
        [SerializeField] private List<int> enemyCountForThatTransform;


        public List<Vector3> GetEnemySpawnTransforms()
        {
            return enemySpawnTransforms;
        }

        public List<EnemyType> GetEnemyToSpawn()
        {
            return enemyToSpawn;
        }

        public List<float> GetSecondsToWaitBetweenSpawn()
        {
            return secondsBetweenSpawn;
        }

        public List<int> GetEnemyCountForThatTransform()
        {
            return enemyCountForThatTransform;
        }
    }
}