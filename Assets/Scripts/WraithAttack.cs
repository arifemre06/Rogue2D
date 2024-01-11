using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class WraithAttack : EnemyRangedAttackObject
{
    void Update()
    {
        var transform1 = transform;
        transform1.position += transform1.up * (Speed * Time.deltaTime);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
}
