using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyRangedAttackObject : MonoBehaviour
    {
        protected float Speed;
        [SerializeField] private float _attackObjectDamage;
        [SerializeField] private RuinStatueData ruinStatueData;

        private void Awake()
        {
            EventManager.RuinHighRiskHighRewardTaken += OnHighRiskHighRewardTaken;
        }

        private void OnDestroy()
        {
            EventManager.RuinHighRiskHighRewardTaken += OnHighRiskHighRewardTaken;
        }

        private void Start()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"),LayerMask.NameToLayer("EnemyAttackObject"),true);

        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public float GetDamage()
        {
            return _attackObjectDamage;
        }
        
        private void OnHighRiskHighRewardTaken()
        {   
            //list order (damage,health,gold drop)
            List<float> highRiskHighRewardData = new List<float>();
            highRiskHighRewardData = ruinStatueData.GetHighRiskHighRewardData();
            _attackObjectDamage *= highRiskHighRewardData[0];
        }

        private void CheckTakenRuins()
        {
            if (TakenRuins.HighRiskHighRewardTaken)
            {
                OnHighRiskHighRewardTaken();
            }
        }
        
    }
}