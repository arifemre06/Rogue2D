using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private float speedArrowOriginal;
    [SerializeField] private float arrowDamage = 5f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
    }

    // Update is called once per frame
    void Update()
    {
        var transform1 = transform;
        transform1.position += transform1.up * (speedArrowOriginal * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
    private void Awake()
    {
        EventManager.AttackDamageRelicCollected += AttackDamageRelicTaken;
    }
    
    private void OnDestroy()
    {
        EventManager.AttackDamageRelicCollected -= AttackDamageRelicTaken;
    }
    private void AttackDamageRelicTaken(float mainModifier)
    {
        arrowDamage = arrowDamage * mainModifier;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {   
        
        if (col.collider.CompareTag("Enemy"))
        {   
            Destroy(gameObject);
            GameObject tempEnemy = col.gameObject;
            enemyscript enemyScript = tempEnemy.GetComponent<enemyscript>();
            enemyScript.SetHealth(arrowDamage);
            
            if (enemyScript.GetHealth() <= 0)
            {
                Destroy(col.gameObject);
            }
        }
    }
}
