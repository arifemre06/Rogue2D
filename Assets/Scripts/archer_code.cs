using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class archer_code : MonoBehaviour
{
    [SerializeField] public float fireRate = 3f;
    [SerializeField] float shootingDistance = 7f;
    [SerializeField] float speedArrow = 5f;
    [SerializeField] float Health = 5f;
    [SerializeField] private float collisionDamage = 40f;
    
    [SerializeField] GameObject arrowPrefab;
    GameObject target;
    bool canShoot = true;
    // Start is called before the first frame update
    private void Awake()
    {
        EventManager.AttackSpeedRelicCollected += AttackSpeedRelicTaken;
    }

    private void OnDestroy()
    {
        EventManager.AttackSpeedRelicCollected -= AttackSpeedRelicTaken;
    }

    // Update is called once per frame
    private void Update ()
    {
        if (canShoot) {
            canShoot = false;
            //Coroutine for delay between shooting
            StartCoroutine("AllowToShoot");
            //array with enemies
            //you can put in start, iff all enemies are in the level at beginn (will be not spawn later)
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy");
            if (allTargets.Length != 0)
            {
                target = allTargets[0];
                //look for the closest
                foreach (GameObject tmpTarget in allTargets)
                {
                    if (Vector2.Distance(transform.position, tmpTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                    {
                        target = tmpTarget;
                    }
                }
                //shoot if the closest is in the fire range
                if (Vector2.Distance(transform.position, target.transform.position) < shootingDistance)
                {
                    
                    Fire();
                }
            }
        }
    }
 
    private void Fire ()
    {
        var position = transform.position;
        Vector3 direction = (target.transform.position - position);
        //link to spawned arrow, you dont need it, if the arrow has own moving script
        Quaternion rotation2 = Quaternion.FromToRotation(transform.up,direction);
        
        //rotation2 *= Quaternion.Euler(0, 0, 180);
        
        GameObject tmpArrow = Instantiate(arrowPrefab, new Vector3(position.x+0.5f,position.y,position.z), rotation2);
    }
 
    IEnumerator AllowToShoot ()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Enemy"))
        {   
            GameObject tempEnemy = col.gameObject;
            enemyscript enemyScript = tempEnemy.GetComponent<enemyscript>();
            enemyScript.SetHealth(collisionDamage);
            Health -= enemyScript.GetCollisionDamage();
            
            if (enemyScript.GetHealth() <= 0)
            {
                Destroy(col.gameObject);
            }
        }
    }

    private void AttackSpeedRelicTaken(float mainModifier)
    {
        fireRate = fireRate * mainModifier;
    }
    
}
