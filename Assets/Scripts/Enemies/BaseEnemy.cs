using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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

        [SerializeField] protected Animator Animator;

        [SerializeField] private RuinStatueData ruinEffectData;

        public const float LevelDamageIncreaseModifier = 0.02f;
        public const float LevelHealthIncreaseModifier = 0.05f;
        public const float LevelGoldIncreaseModifier = 0.03f;
        
        protected GameObject Character;
        
        private float _enemyMaxHealth;
        private int _notHitForOneSecCount;
        private bool _oneSecondPassed;

        private void Awake()
        {
            EventManager.RuinHighRiskHighRewardTaken += OnHighRiskHighRewardTaken;
            EventManager.RuinGiveMeTrioTaken += OnGiveTrioTaken;
            EventManager.NextLevel += OnNextLevel;
        }

        private void OnDestroy()
        {
            EventManager.RuinHighRiskHighRewardTaken -= OnHighRiskHighRewardTaken;
            EventManager.RuinGiveMeTrioTaken -= OnGiveTrioTaken;
            EventManager.NextLevel -= OnNextLevel;
        }

        private void Start()
        {   
            Character = GameObject.FindGameObjectWithTag("Player");
            _enemyMaxHealth = enemyHealth;
            enemyCanvas.enabled = false;
            _oneSecondPassed = true;
            UpdateBaseStatsAccordingToLevelIndex(gameController.LevelIndex);
            CheckTakenRuins();
            
        }

        protected virtual void  Update()
            {
                Vector3 direction = Character.transform.position - transform.position;
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
                    //bura sıkıntı cıkarabılır
                    EventManager.OnEnemyKilled(this,goldDrop, experienceDrop);
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
            
            private void OnHighRiskHighRewardTaken()
            {
                //list order (damage,health,gold drop)
                List<float> highRiskHighRewardData = new List<float>();
                highRiskHighRewardData = ruinEffectData.GetHighRiskHighRewardData();
                enemyColDamage *= highRiskHighRewardData[0];
                enemyHealth *= highRiskHighRewardData[1];
                float tempGoldDrop = goldDrop * highRiskHighRewardData[2];
                goldDrop = (int)tempGoldDrop;
            }

            private void OnGiveTrioTaken()
            {
                
            }
            
            private void CheckTakenRuins()
            {
                if (TakenRuins.HighRiskHighRewardTaken)
                {
                    OnHighRiskHighRewardTaken();
                }

                if (TakenRuins.GiveMeTrioTaken)
                {
                    OnGiveTrioTaken();
                }
            }

            protected virtual void OnNextLevel(int levelIndex)
            {
                UpdateBaseStatsAccordingToLevelIndex(levelIndex);
            }
            private void UpdateBaseStatsAccordingToLevelIndex(int levelIndex)
            {
                enemyColDamage = enemyColDamage + enemyColDamage * (levelIndex * LevelDamageIncreaseModifier);
                enemyHealth = enemyHealth + enemyHealth *(levelIndex * LevelHealthIncreaseModifier);
                float tempGoldDrop = goldDrop + goldDrop * (levelIndex * LevelGoldIncreaseModifier);
                goldDrop = (int)tempGoldDrop;
            }
    }
}