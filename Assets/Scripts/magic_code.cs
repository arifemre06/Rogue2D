using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_code : MonoBehaviour
{   
    [SerializeField] private float magicDamage = 5f;
    [SerializeField] private float destroyTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, destroyTime); 
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
        magicDamage = magicDamage * mainModifier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {   
           
            GameObject tempEnemy = other.gameObject;
            enemyscript enemyScript = tempEnemy.GetComponent<enemyscript>();
            enemyScript.SetHealth(magicDamage);
            
            if (enemyScript.GetHealth() <= 0)
            {
                Destroy(other.gameObject);
                
            }
        }
    }

    
}
