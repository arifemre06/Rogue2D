using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace DefaultNamespace.Heroes
{
    public class Mage : BaseHero
    {
        [SerializeField] MagicAttackObject magicPrefab;

        protected override void Attack(GameObject target)
        {   
            Animator.Play("Attack");
            Quaternion rotation2 = transform.rotation;
            rotation2 *= Quaternion.Euler(0, 0, 90);
            MagicAttackObject tmpMagic = Instantiate(magicPrefab, target.transform.position, rotation2);
            tmpMagic.SetDamage(Damage);
            
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            Animator.Play("Hurt");
        }
    }
}
