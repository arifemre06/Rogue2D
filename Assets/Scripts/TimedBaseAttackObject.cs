using System;
using Enemies;
using UnityEngine;

namespace DefaultNamespace
{
    public class TimedBaseAttackObject : BaseAttackObject
    {
        [SerializeField] private float disappearCooldown;
        
        protected override void Start()
        {
            base.Start();
            Destroy(gameObject,disappearCooldown);
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                GameObject tempEnemy = col.gameObject;
                BaseEnemy enemyScript = tempEnemy.GetComponent<BaseEnemy>();
                enemyScript.SetHealth(Damage);
                
                if (enemyScript.GetHealth() <= 0)
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}