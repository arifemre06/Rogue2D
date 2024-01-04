using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Arrow : BaseAttackObject
{
    [SerializeField] private LayerMask characterLayer;
    private float _speedArrowOriginal;
    
    void Update()
    {
        var transform1 = transform;
        transform1.position += transform1.up * (_speedArrowOriginal * Time.deltaTime);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetArrowSpeed(float speed)
    {
        _speedArrowOriginal = speed;
    }
    
}
