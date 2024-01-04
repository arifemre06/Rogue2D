using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyscript : MonoBehaviour
{
    [SerializeField] private float enemyHealth = 100;
    
    [SerializeField] private float enemySpeed = 0.05f;
    
    [SerializeField] private float enemyColDamage = 4f;
    
    [SerializeField] private int goldDrop = 5;
    
    [SerializeField] private int experienceDrop = 10;
    
    
    private GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = character.transform.position - transform.position;
        Quaternion rotation2 = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * (enemySpeed * Time.deltaTime); 
    }
    
    
    public void SetHealth(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            EventManager.OnEnemyKilled(goldDrop, experienceDrop);
            Destroy(this.gameObject);
        }
    }

    public float GetHealth()
    {
        return enemyHealth;
    }

    public float GetCollisionDamage()
    {
        return enemyColDamage;
    }

    public void SetSpeed(float speed)
    {
        enemySpeed = speed;
    }
}
