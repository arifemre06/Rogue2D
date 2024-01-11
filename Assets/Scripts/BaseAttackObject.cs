using Enemies;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BaseAttackObject : MonoBehaviour
    {
        protected float Damage;

        protected virtual void Start()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"),LayerMask.NameToLayer("AttackObject"),true);
        }
        
        public void SetDamage(float damage)
        {
            Damage = damage;
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {   
        
            if (col.collider.CompareTag("Enemy"))
            {   
                Destroy(gameObject);
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