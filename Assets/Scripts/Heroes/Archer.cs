using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace DefaultNamespace.Heroes
{
    public class Archer : BaseHero
    {
        [SerializeField] private float speedArrow;
        [SerializeField] Arrow arrowPrefab;
        
        protected override void Attack(BaseEnemy target)
        {
            Animator.Play("Attack");
            var position = transform.position;
            Vector3 direction = (target.transform.position - position);
            
            Quaternion rotation2 = Quaternion.FromToRotation(transform.up,direction);
            Arrow tmpArrow = Instantiate(arrowPrefab, new Vector3(position.x+0.5f,position.y,position.z), rotation2);
            tmpArrow.SetDamage(Damage);
            tmpArrow.SetArrowSpeed(speedArrow);
            
        }
    }

}
