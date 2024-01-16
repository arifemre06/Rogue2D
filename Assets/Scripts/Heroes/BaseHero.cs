using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Enemies;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public abstract class BaseHero : MonoBehaviour
    {   
        [SerializeField] protected float Health;
        [SerializeField] protected float FireCooldown;
        [SerializeField] protected float CollisionDamage;
        [SerializeField] protected float Damage;
        [SerializeField] protected float ShootingDistance;
        [SerializeField] protected float DamageProtectionRegenCooldown;
        protected float LifeSteal = 0;
        protected float LifeRegen = 0;
        protected int MaxDamageProtectionAmount = 0;
        protected int CurrentDamageProtectionAmount = 0;
        protected float DodgeChance = 0;

        [SerializeField] protected Animator Animator;
        
        [SerializeField] protected Image healthBar;

        [SerializeField] protected Canvas heroCanvas;

        private float _heroMaxHealth;

        private List<BaseEnemy> allTargets;
        protected bool CanShoot = true;
        protected bool CanLifeRegen = true;
        protected bool CanDamageProtection = true;

        protected bool CollidedWithEnemies;
        
        private void Awake()
        {
            EventManager.GameStarted += OnGameStarted;
            EventManager.AttackSpeedRelicCollected += OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected += OnAttackDamageRelicTaken;
            EventManager.LifeStealRelicCollected += OnLifeStealRelicTaken;
            EventManager.LifeRegenRelicCollected += OnLifeRegenRelicCollected;
            EventManager.MovementSpeedRelicCollected += OnMovementSpeedRelicCollected;
            EventManager.MoreShootingDistanceRelicCollected += OnMoreShootingDistanceRelicCollected;
            EventManager.DodgeChanceRelicCollected += OnDodgeChanceRelicCollected;
            EventManager.DamageProtectionRelicCollected += OnDamageProtectionRelicCollected;
            EventManager.EnemySpawned += OnEnemySpawnedUpdateEnemiesTransform;
            EventManager.EnemyKilled += OnEnemyKilledUpdateEnemiesTransform;
            CanShoot = true;
        }

        private void OnDestroy()
        {
            EventManager.GameStarted -= OnGameStarted;
            EventManager.AttackSpeedRelicCollected -= OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected -= OnAttackDamageRelicTaken;
            EventManager.LifeStealRelicCollected -= OnLifeStealRelicTaken;
            EventManager.LifeRegenRelicCollected -= OnLifeRegenRelicCollected;
            EventManager.MovementSpeedRelicCollected -= OnMovementSpeedRelicCollected;
            EventManager.MoreShootingDistanceRelicCollected += OnMoreShootingDistanceRelicCollected;
            EventManager.DodgeChanceRelicCollected += OnDodgeChanceRelicCollected;
            EventManager.DamageProtectionRelicCollected += OnDamageProtectionRelicCollected;
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
            if (LifeRegen > 0 && CanLifeRegen && (Health < _heroMaxHealth))
            {
                CanLifeRegen = false;
                StartCoroutine(LifeRegenCooldown());
            }

            if (MaxDamageProtectionAmount > 0 && CanDamageProtection &&
                (CurrentDamageProtectionAmount < MaxDamageProtectionAmount))
            {
                CanDamageProtection = false;
                StartCoroutine(DamageProtectionCooldown());
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
        
        protected IEnumerator LifeRegenCooldown()
        {
            yield return new WaitForSeconds(1);
            RegenLife();
            CanLifeRegen = true;
        }

        protected IEnumerator DamageProtectionCooldown()
        {
            yield return new WaitForSeconds(DamageProtectionRegenCooldown);
            CurrentDamageProtectionAmount += 1;
            CanDamageProtection = true;
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
                StartCoroutine(WaitBetweenEnemyCollides());
                GameObject tempEnemy = col.gameObject;
                BaseEnemy enemyScript = tempEnemy.GetComponent<BaseEnemy>();
                enemyScript.SetHealth(CollisionDamage);
                if (!CheckDodge())
                {
                    if (CheckDamageProtection())
                    {
                        Debug.Log("hasardan korunduk");
                    }
                    else
                    {
                        Health -= enemyScript.GetCollisionDamage();
                        UpdateHealthBar(Health);
                    }
                }
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
                    StartCoroutine(WaitBetweenEnemyCollides());
                    GameObject tempEnemy = col.gameObject;
                    EnemyRangedAttackObject rangedAttackScript = tempEnemy.GetComponent<EnemyRangedAttackObject>();
                    
                    if (!CheckDodge())
                    {
                        if (CheckDamageProtection())
                        {
                            Debug.Log("hasardan korunduk");
                        }
                        else
                        {
                            Health -= rangedAttackScript.GetDamage();
                            UpdateHealthBar(Health);
                        }
                    }
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

        private void RegenLife()
        {
            Health += _heroMaxHealth * LifeRegen;
            UpdateHealthBar(Health);
        }
        
        private void OnGameStarted()
        {
            Health = _heroMaxHealth;
            UpdateHealthBar(Health);
        }

        private bool CheckDodge()
        {
            if (DodgeChance > 0)
            {
                Debug.Log("dodgelama sorgusuna hosgeldınız");
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < DodgeChance * 100)
                {
                    Debug.Log("saldırıyı dodgeladık");
                    return true;
                } 
            }
            return false;
        }

        private bool CheckDamageProtection()
        {
            if (CurrentDamageProtectionAmount > 0)
            {
                CurrentDamageProtectionAmount -= 1;
                return true;
            }

            return false;
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
        
        private void OnLifeStealRelicTaken(float obj)
        {
            LifeSteal = LifeSteal + obj;
        }
        
        private void OnMovementSpeedRelicCollected(float obj)
        {
            
        }

        private void OnLifeRegenRelicCollected(float obj)
        {
            LifeRegen = obj;
        }
        
        private void OnDamageProtectionRelicCollected(int obj)
        {
            MaxDamageProtectionAmount = obj;
        }

        private void OnDodgeChanceRelicCollected(float obj)
        {
            DodgeChance = obj;
        }

        private void OnMoreShootingDistanceRelicCollected(float obj)
        {
            ShootingDistance *= obj;
        }

        
        
    }
}