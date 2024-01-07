using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyscript : MonoBehaviour
{
    [SerializeField] private float enemyHealth = 100;
    
    [SerializeField] private float enemySpeed = 0.05f;
    
    [SerializeField] private float enemyColDamage = 4f;
    
    [SerializeField] private int goldDrop = 5;
    
    [SerializeField] private int experienceDrop = 10;

    [SerializeField] private Image healthBar;

    [SerializeField] private Canvas enemyCanvas;

    private float _enemyMaxHealth;
    private int _notHitForOneSecCount;
    private bool _oneSecondPassed;
    
    
    private GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        _enemyMaxHealth = enemyHealth;
        enemyCanvas.enabled = false;
        _oneSecondPassed = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = character.transform.position - transform.position;
        Quaternion rotation2 = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * (enemySpeed * Time.deltaTime);
        if (_oneSecondPassed)
        {
            StartCoroutine(HideHealthBarUI());
        }
    }
    
    
    public void SetHealth(float damage)
    {
        enemyHealth -= damage;
        UpdateHealthBar(enemyHealth);
        if (enemyHealth <= 0)
        {
            EventManager.OnEnemyKilled(this.gameObject,goldDrop, experienceDrop);
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

    private void UpdateHealthBar(float currentHealth)
    {
        _notHitForOneSecCount = 0;
        enemyCanvas.enabled = true;
        healthBar.fillAmount = currentHealth / _enemyMaxHealth;
    }

    private IEnumerator HideHealthBarUI()
    {
        _oneSecondPassed = false;
        yield return new WaitForSeconds(1);
        _oneSecondPassed = true;
        Debug.Log("buraya ne sıklıkla giriyoz "+_notHitForOneSecCount);
        _notHitForOneSecCount += 1;
        if (_notHitForOneSecCount > 2)
        {
            enemyCanvas.enabled = false;
        }
    }
}
