using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyRangedAttackObject : MonoBehaviour
    {
        protected float Speed;
        private float _attackObjectDamage;


        private void Start()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"),LayerMask.NameToLayer("EnemyAttackObject"),true);

        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void SetDamage(float value)
        {
            _attackObjectDamage = value;
        }
        public float GetDamage()
        {
            return _attackObjectDamage;
        }
        
        
    }
}