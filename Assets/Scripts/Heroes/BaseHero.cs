using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public abstract class BaseHero : MonoBehaviour
    {   
        [SerializeField] protected float Health;
        [SerializeField] protected float FireCooldown;
        [SerializeField] protected float CollisionDamage;
        [SerializeField] protected float Damage;
        [SerializeField] protected float ShootingDistance;

        [SerializeField] protected Animator Animator;
        
        [SerializeField] protected Image healthBar;

        [SerializeField] protected Canvas heroCanvas;

        private float _heroMaxHealth;

        private List<BaseEnemy> allTargets;
        protected bool CanShoot = true;

        protected bool CollidedWithEnemies;
        
        private void Awake()
        {
            EventManager.GameStarted += OnGameStarted;
            EventManager.AttackSpeedRelicCollected += OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected += OnAttackDamageRelicTaken;
            EventManager.EnemySpawned += OnEnemySpawnedUpdateEnemiesTransform;
            EventManager.EnemyKilled += OnEnemyKilledUpdateEnemiesTransform;
            CanShoot = true;
        }

        private void OnDestroy()
        {
            EventManager.GameStarted -= OnGameStarted;
            EventManager.AttackSpeedRelicCollected -= OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected -= OnAttackDamageRelicTaken;
            EventManager.EnemySpawned -= OnEnemySpawnedUpdateEnemiesTransform;
            EventManager.EnemyKilled -= OnEnemyKilledUpdateEnemiesTransform;
        }

        private void Start()
        {
            allTargets = new List<BaseEnemy>();
            _heroMaxHealth = Health;
            heroCanvas.enabled = false;
        }

        protected abstract void Attack(BaseEnemy target);
        

        protected void Update()
        {
            if (CanShoot)
            {
                BaseEnemy target = GetClosestTargetInRange();
                CanShoot = false;
                StartCoroutine(StartAttackCooldown());
                if (target != null)
                {
                    Attack(target);
                }
            }
        }

        protected BaseEnemy GetClosestTargetInRange()
        {
            if (allTargets.Count != 0)
            {
                BaseEnemy target = allTargets[0];
                    //look for the closest
                    foreach (BaseEnemy tmpTarget in allTargets)
                    {
                        if (Vector2.Distance(transform.position, tmpTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                        {
                            target = tmpTarget;
                        }
                    }
                    //shoot if the closest is in the fire range
                    if (Vector2.Distance(transform.position, target.transform.position) < ShootingDistance)
                    {
                        return target;
                    }
            }
            return null;
        }
        
        protected IEnumerator StartAttackCooldown()
        {
            yield return new WaitForSeconds(FireCooldown);
            CanShoot = true;
        }
        
        private void OnEnemySpawnedUpdateEnemiesTransform(BaseEnemy obj)
        {
            allTargets.Add(obj);
        }
        
        private void OnEnemyKilledUpdateEnemiesTransform(BaseEnemy enemy,int arg1, int arg2)
        {
            allTargets.Remove(enemy);
        }
        
        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag("Enemy") && !CollidedWithEnemies)
            {
                CollidedWithEnemies = true;
                GameObject tempEnemy = col.gameObject;
                BaseEnemy enemyScript = tempEnemy.GetComponent<BaseEnemy>();
                enemyScript.SetHealth(CollisionDamage);
                Health -= enemyScript.GetCollisionDamage();
                UpdateHealthBar(Health);

                if (enemyScript.GetHealth() <= 0)
                {
                    Destroy(col.gameObject);
                }
            }
            if (col.collider.CompareTag("EnemyAttack"))
            {
                if (!CollidedWithEnemies)
                {
                    CollidedWithEnemies = true;
                    GameObject tempEnemy = col.gameObject;
                    EnemyRangedAttackObject rangedAttackScript = tempEnemy.GetComponent<EnemyRangedAttackObject>();
                    Health -= rangedAttackScript.GetDamage();
                    UpdateHealthBar(Health);
                    
                    Debug.Log("ee biz buraya gırıyozzzz???");
                }
                Destroy(col.gameObject);
            }
        }
        
        private IEnumerator WaitBetweenEnemyCollides()
        {
            yield return new WaitForSeconds(0.1f);
            CollidedWithEnemies = false;
        }
        
        private void UpdateHealthBar(float currentHealth)
        {
            heroCanvas.enabled = true;
            healthBar.fillAmount = currentHealth / _heroMaxHealth;
        }
        
        private void OnGameStarted()
        {
            Health = _heroMaxHealth;
            UpdateHealthBar(Health);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        protected void OnAttackDamageRelicTaken(float mainModifier)
        {
            Damage = Damage * mainModifier;
            Debug.Log("main modifier "+mainModifier);
        }

        protected void OnAttackSpeedRelicTaken(float mainModifier)
        {
            FireCooldown = FireCooldown * mainModifier;
        }
        
        
    }
}