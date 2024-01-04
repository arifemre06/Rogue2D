using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class warriorAttackCode : MonoBehaviour
{
    [SerializeField] private float warriorAttackDamage = 5f;
    [SerializeField] private float destroyTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
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
        warriorAttackDamage = warriorAttackDamage * mainModifier;
    }

    

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, destroyTime); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {   
            
            GameObject tempEnemy = other.gameObject;
            enemyscript enemyScript = tempEnemy.GetComponent<enemyscript>();
            enemyScript.SetHealth(warriorAttackDamage);
            
            if (enemyScript.GetHealth() <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
