using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyRangedAttackObject : MonoBehaviour
    {
        protected float Speed;
        [SerializeField] private float _attackObjectDamage;
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
        
    }
}