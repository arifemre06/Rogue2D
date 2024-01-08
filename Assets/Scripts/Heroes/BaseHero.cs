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

        private List<GameObject> allTargets;
        protected bool CanShoot = true;
        
        private void Awake()
        {
            EventManager.AttackSpeedRelicCollected += OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected += OnAttackDamageRelicTaken;
            EventManager.EnemySpawned += OnEnemySpawnedUpdateEnemiesTransform;
            EventManager.EnemyKilled += OnEnemyKilledUpdateEnemiesTransform;
            CanShoot = true;
        }

        private void OnDestroy()
        {
            EventManager.AttackSpeedRelicCollected -= OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected -= OnAttackDamageRelicTaken;
            EventManager.EnemySpawned -= OnEnemySpawnedUpdateEnemiesTransform;
            EventManager.EnemyKilled -= OnEnemyKilledUpdateEnemiesTransform;
        }

        private void Start()
        {
            allTargets = new List<GameObject>();
            _heroMaxHealth = Health;
            heroCanvas.enabled = false;
        }

        protected abstract void Attack(GameObject target);
        

        protected void Update()
        {
            if (CanShoot)
            {
                GameObject target = GetClosestTargetInRange();
                CanShoot = false;
                StartCoroutine(StartAttackCooldown());
                if (target != null)
                {
                    Attack(target);
                }
            }
        }

        protected GameObject GetClosestTargetInRange()
        {
            if (allTargets.Count != 0)
            {
                GameObject target = allTargets[0];
                    //look for the closest
                    foreach (GameObject tmpTarget in allTargets)
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
        
        private void OnEnemySpawnedUpdateEnemiesTransform(GameObject obj)
        {
            allTargets.Add(obj);
        }
        
        private void OnEnemyKilledUpdateEnemiesTransform(GameObject enemy,int arg1, int arg2)
        {
            allTargets.Remove(enemy);
        }
        
        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag("Enemy"))
            {   
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
        }
        
        private void UpdateHealthBar(float currentHealth)
        {
            heroCanvas.enabled = true;
            healthBar.fillAmount = currentHealth / _heroMaxHealth;
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