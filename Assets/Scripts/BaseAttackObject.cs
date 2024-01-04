using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BaseAttackObject : MonoBehaviour
    {
        protected float Damage;
        
        protected virtual void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");     
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
                enemyscript enemyScript = tempEnemy.GetComponent<enemyscript>();
                enemyScript.SetHealth(Damage);
            
                if (enemyScript.GetHealth() <= 0)
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}