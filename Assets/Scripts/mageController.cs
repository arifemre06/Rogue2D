using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mageController : MonoBehaviour
{   
    [SerializeField] public float fireRate = 3f;
    [SerializeField] float shootingDistance = 7f;
    [SerializeField] float Health = 5f;
    [SerializeField] private float collisionDamage = 40f;
    
    [SerializeField] GameObject magicPrefab;
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
            
            Quaternion rotation2 = transform.rotation;
            rotation2 *= Quaternion.Euler(0, 0, 90);
            GameObject tmpMagic = Instantiate(magicPrefab, target.transform.position, rotation2);
            
        }
     
        IEnumerator AllowToShoot ()
        {
            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }
    
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.tag == "Enemy")
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
