using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Enemies;
using Mono.Cecil;
using ScriptableObjectsScripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public abstract class BaseHero : MonoBehaviour
    { 
        protected float Health;
        [SerializeField] protected float FireCooldown;
        [SerializeField] protected float CollisionDamage;
        [SerializeField] protected float Damage;
        [SerializeField] protected float ShootingDistance;
        [SerializeField] protected float DamageProtectionRegenCooldown;
        
        [SerializeField] protected float BaseDamage;
        [SerializeField] protected float BaseShootingDistance;
        
            
        protected float LifeSteal = 0;
        protected float LifeRegen = 0;
        protected const int LifeRegenTimeInterval = 5;
        protected int MaxDamageProtectionAmount = 0;
        protected int CurrentDamageProtectionAmount = 0;
        protected float DodgeChance = 0;

        [SerializeField] protected Animator Animator;
        
        [SerializeField] protected Image healthBar;

        [SerializeField] protected Canvas heroCanvas;

        [SerializeField] private float _heroMaxHealth;

        private List<BaseEnemy> allTargets;
        protected bool CanShoot = true;
        protected bool CanLifeRegen = true;
        protected bool CanDamageProtection = true;

        protected bool CollidedWithEnemies;
        private bool _isDead;
        
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
            EventManager.NextLevel += OnNextLevel;
            EventManager.GameOver += OnGameOver;
            CanShoot = true;
            gameController.HeroCountOnScene += 1;
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
            EventManager.NextLevel -= OnNextLevel;
            EventManager.GameOver -= OnGameOver;
        }


        private void Start()
        {
            allTargets = new List<BaseEnemy>();
            //_heroMaxHealth = Health;
            Health = _heroMaxHealth;
            UpdateHealthBar(Health);
        }

        protected abstract void Attack(BaseEnemy target);
        

        protected void Update()
        {
            if (!_isDead)
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
        }
        
        private void OnGameStarted()
        {
            ReviveHero();
        }
        private void OnNextLevel(int obj)
        {
            ReviveHero();
        }
        
        private void OnGameOver()
        {
            ClearTargetArray();
            Destroy(gameObject);
        }

        private void ClearTargetArray()
        {
            allTargets.Clear();
        }

        private void ReviveHero()
        {
            if (_isDead)
            {
                CanShoot = true;
                gameObject.GetComponent<Collider2D>().enabled = true;
                heroCanvas.enabled = true;
                _isDead = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameController.HeroCountOnScene += 1;
            }
            Health = _heroMaxHealth;
            UpdateHealthBar(Health);
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
            yield return new WaitForSeconds(LifeRegenTimeInterval);
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
                        if (Health - enemyScript.GetCollisionDamage() < 0)
                        {
                            if (gameController.HeroCountOnScene <= 1)
                            {
                                EventManager.OnGameOver();
                            }

                            HeroDeadState();
                            //Destroy(gameObject);
                        }
                        else
                        {
                            Health -= enemyScript.GetCollisionDamage();
                            UpdateHealthBar(Health);
                        }
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
                            if (Health - rangedAttackScript.GetDamage() < 0)
                            {
                                if (gameController.HeroCountOnScene <= 1)
                                {
                                    EventManager.OnGameOver();
                                }

                                HeroDeadState();
                                //Destroy(gameObject);
                            }
                            else
                            {
                                Health -= rangedAttackScript.GetDamage();
                                UpdateHealthBar(Health);
                            }
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
            healthBar.fillAmount = currentHealth / _heroMaxHealth;
        }

        private void RegenLife()
        {
            if ((Health + _heroMaxHealth * LifeRegen) > _heroMaxHealth)
            {
                Health = _heroMaxHealth;
            }
            else
            {
                Health += _heroMaxHealth * LifeRegen;
            }
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
        
        protected void OnAttackDamageRelicTaken(float mainModifier,int amount)
        {
            Damage = LineerStatsIncrease(BaseDamage, mainModifier, amount);
        }

        protected void OnAttackSpeedRelicTaken(float mainModifier,int amount)
        {   
            //we will turn back here to look for adjustment
            //attack speed doesnt use basestats so it doesnt have a lineer increase
            float decreaseAmount = FireCooldown * mainModifier;
            FireCooldown = FireCooldown - decreaseAmount;
        }
        
        private void OnLifeStealRelicTaken(float mainModifier,int amount)
        {
            LifeSteal = HyperBolicIncrease(mainModifier, amount);
        }
        
        private void OnMovementSpeedRelicCollected(float mainModifier,int amount)
        {
            
        }

        private void OnLifeRegenRelicCollected(float mainModifier,int amount)
        {
            LifeRegen = HyperBolicIncrease(mainModifier, amount);
        }
        
        private void OnDamageProtectionRelicCollected(int obj)
        {
            MaxDamageProtectionAmount = obj;
        }

        private void OnDodgeChanceRelicCollected(float mainModifier,int amount)
        {
            DodgeChance = HyperBolicIncrease(mainModifier, amount);
        }

        private void OnMoreShootingDistanceRelicCollected(float mainModifier,int amount)
        {   
            //we are increasing range with percentage we add 1 to the hyperbolic increase because we take base range %100
            ShootingDistance = BaseShootingDistance * (1 + HyperBolicIncrease(mainModifier, amount));
        }

        private float LineerStatsIncrease(float baseStat ,float mainModifier,int amount)
        {
            return baseStat + baseStat * mainModifier * amount;
        }
        //In HyperBolic Stacking we take baseStat = 1;
        private float HyperBolicIncrease(float mainModifier, int amount)
        {
            return 1 - 1 / (1 + mainModifier * amount);
        }

        private void HeroDeadState()
        {
            _isDead = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            heroCanvas.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameController.HeroCountOnScene -= 1;
        }
        
        
    }
}