using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Heroes
{
    public class Warrior : BaseHero
    {
        [SerializeField] WarriorAttackObject warriorattackPrefab;

        protected override void Attack(GameObject target)
        {
            Animator.Play("Attack");
            var middlePoint = (target.transform.position + transform.position) / 2;
            WarriorAttackObject tmpAttack = Instantiate(warriorattackPrefab, middlePoint,
                Quaternion.LookRotation(target.transform.position - middlePoint) * Quaternion.Euler(0, 90, 0));
            tmpAttack.SetDamage(Damage);
        }
        
        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            Animator.Play("Hurt");
        }
    }
}
