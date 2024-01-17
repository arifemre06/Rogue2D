using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace Enemies
{
    public class RangedEnemy : BaseEnemy
    {
        [SerializeField] protected EnemyRangedAttackObject AttackObjectPrefab;
        
        [SerializeField] protected float AttackObjectSpeed;
        [SerializeField] protected float FireCooldown;
        [SerializeField] private float range;
        [SerializeField] private float attackObjectDamage;
        private bool _canShoot = true;
        protected void Attack()
        {
            
            Animator.Play("Attack");
            var position = transform.position;
            Vector3 direction = (Character.transform.position - position);
            
            Quaternion rotation2 = Quaternion.FromToRotation(transform.up,direction);
            EnemyRangedAttackObject tmpObject = Instantiate(AttackObjectPrefab, new Vector3(position.x,position.y,position.z), rotation2);
            tmpObject.SetSpeed(AttackObjectSpeed);
            tmpObject.SetDamage(attackObjectDamage);
        }

         protected override void Update()
        {   
            base.Update();
            if (_canShoot && IsTargetInRange())
            {
                _canShoot = false;
                StartCoroutine(StartAttackCooldown());
                if (Character != null)
                {
                    Attack();
                }
            }
        }
        protected IEnumerator StartAttackCooldown()
        {
            yield return new WaitForSeconds(FireCooldown);
            _canShoot = true;
        }

        private bool IsTargetInRange()
        {
            if (Vector2.Distance(transform.position, Character.transform.position) < range)
            {
                return true;
            }

            return false;
        }

        protected override void OnNextLevel(int levelIndex)
        {
            base.OnNextLevel(levelIndex);
            attackObjectDamage = attackObjectDamage + (attackObjectDamage * levelIndex * LevelDamageIncreaseModifier);
        }
    }
}