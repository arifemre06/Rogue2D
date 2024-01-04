using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class warriorController : MonoBehaviour
{
    [SerializeField] private float fireRate = 2f;
    [SerializeField] float shootingDistance = 3f;
    [SerializeField] float Health = 10f;
    [SerializeField] private float collisionDamage = 70f;
    
    [SerializeField] GameObject warriorattackPrefab;
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
    void Update ()
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
     
        void Fire ()
        {
            
            Quaternion rotation2 = transform.rotation;
            rotation2 *= Quaternion.Euler(90, 0, 0);
            var middlePoint = (target.transform.position + transform.position)/2;
            GameObject tmpMagic = Instantiate(warriorattackPrefab, middlePoint, Quaternion.LookRotation(target.transform.position - middlePoint)*Quaternion.Euler(0, 90, 0));
            
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
