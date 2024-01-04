using System;
using System.Collections;
using Mono.Cecil;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BaseHero : MonoBehaviour
    {   
        [SerializeField] protected float Health;
        [SerializeField] protected float FireCooldown;
        [SerializeField] protected float CollisionDamage;
        [SerializeField] protected float Damage;
        [SerializeField] protected float ShootingDistance;
        
        protected bool CanShoot = true;
        
        private void Awake()
        {
            EventManager.AttackSpeedRelicCollected += OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected += OnAttackDamageRelicTaken;
            CanShoot = true;
        }
        
        private void OnDestroy()
        {
            EventManager.AttackSpeedRelicCollected -= OnAttackSpeedRelicTaken;
            EventManager.AttackDamageRelicCollected -= OnAttackDamageRelicTaken;
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
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy");
            if (allTargets.Length != 0)
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
        
        protected void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag("Enemy"))
            {   
                GameObject tempEnemy = col.gameObject;
                enemyscript enemyScript = tempEnemy.GetComponent<enemyscript>();
                enemyScript.SetHealth(CollisionDamage);
                Health -= enemyScript.GetCollisionDamage();
            
                if (enemyScript.GetHealth() <= 0)
                {
                    Destroy(col.gameObject);
                }
            }
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        protected void OnAttackDamageRelicTaken(float mainModifier)
        {
            Damage = Damage * mainModifier;
        }

        protected void OnAttackSpeedRelicTaken(float mainModifier)
        {
            FireCooldown = FireCooldown * mainModifier;
        }
        
        
    }
}