using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "RuinData", menuName = "Game/RuinData", order = 0)]
    public class RuinStatueData : ScriptableObject
    {
        //HighRiskHighReward Data
        [SerializeField] private float highRiskHighRewardenemyDamageMultiplier;
        [SerializeField] private float highRiskHighRewardenemyHealthMultiplier;
        [SerializeField] private float highRiskHighRewardenemyGoldDropMultiplier;
        [SerializeField] private float giveMeTrieDataDamageMultiplier;
        private List<float> _highRiskHighRewardData;
        private List<float> _giveMeTrieData;
        private void Awake()
        {
            _highRiskHighRewardData = new List<float>
            {
                highRiskHighRewardenemyDamageMultiplier,
                highRiskHighRewardenemyHealthMultiplier,
                highRiskHighRewardenemyGoldDropMultiplier
            };
            _giveMeTrieData = new List<float>
            {
                giveMeTrieDataDamageMultiplier
            };
        }
        
        public List<float> GetHighRiskHighRewardData()
        {
            _highRiskHighRewardData = new List<float>
            {
                highRiskHighRewardenemyDamageMultiplier,
                highRiskHighRewardenemyHealthMultiplier,
                highRiskHighRewardenemyGoldDropMultiplier
            };
            return _highRiskHighRewardData;
        }
        
        public List<float> GetGiveMeTrioData()
        {
            _giveMeTrieData = new List<float>
            {
                giveMeTrieDataDamageMultiplier
            };
            return _giveMeTrieData;
        }
    }
}