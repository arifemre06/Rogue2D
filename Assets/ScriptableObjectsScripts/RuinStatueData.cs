using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "RuinData", menuName = "Game/RuinData", order = 0)]
    public class RuinStatueData : ScriptableObject
    {
        //HighRiskHighReward Data
        [SerializeField] private float enemyDamageMultiplier;
        [SerializeField] private float enemyHealthMultiplier;
        [SerializeField] private float enemyGoldDropMultiplier;
        private List<float> _highRiskHighRewardData;
        private void Awake()
        {
            _highRiskHighRewardData = new List<float>
            {
                enemyDamageMultiplier,
                enemyHealthMultiplier,
                enemyGoldDropMultiplier
            };
        }


        public List<float> GetHighRiskHighRewardData()
        {
            return _highRiskHighRewardData;
        }
    }
}