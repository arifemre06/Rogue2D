using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        [SerializeField] protected float enemyHealth = 100;
    
        [SerializeField] protected float enemySpeed = 0.05f;
    
        [SerializeField] protected float enemyColDamage = 4f;
    
        [SerializeField] protected int goldDrop = 5;
    
        [SerializeField] protected int experienceDrop = 10;

        [SerializeField] protected Image healthBar;

        [SerializeField] protected Canvas enemyCanvas;
        
        private GameObject character;
        
        private float _enemyMaxHealth;
        private int _notHitForOneSecCount;
        private bool _oneSecondPassed;
        
        void Start()
        {
            character = GameObject.FindGameObjectWithTag("Player");
            _enemyMaxHealth = enemyHealth;
            enemyCanvas.enabled = false;
            _oneSecondPassed = true;
        }
        
        void Update()
            {
                Vector3 direction = character.transform.position - transform.position;
                Quaternion rotation2 = Quaternion.LookRotation(direction);
                transform.position += direction.normalized * (enemySpeed * Time.deltaTime);
                if (_oneSecondPassed)
                {
                    StartCoroutine(HideHealthBarUI());
                }
            }
            
            
            public void SetHealth(float damage)
            {
                enemyHealth -= damage;
                UpdateHealthBar(enemyHealth);
                if (enemyHealth <= 0)
                {
                    EventManager.OnEnemyKilled(this.gameObject,goldDrop, experienceDrop);
                    Destroy(this.gameObject);
                }
            }
        
            public float GetHealth()
            {
                return enemyHealth;
            }
        
            public float GetCollisionDamage()
            {
                return enemyColDamage;
            }
        
            public void SetSpeed(float speed)
            {
                enemySpeed = speed;
            }
        
            private void UpdateHealthBar(float currentHealth)
            {
                _notHitForOneSecCount = 0;
                enemyCanvas.enabled = true;
                healthBar.fillAmount = currentHealth / _enemyMaxHealth;
            }
        
            private IEnumerator HideHealthBarUI()
            {
                _oneSecondPassed = false;
                yield return new WaitForSeconds(1);
                _oneSecondPassed = true;
                _notHitForOneSecCount += 1;
                if (_notHitForOneSecCount > 2)
                {
                    enemyCanvas.enabled = false;
                }
            }
    }
}