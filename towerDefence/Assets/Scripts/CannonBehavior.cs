using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    /*
     * Firing
     */
     [Header("Firing")]
    public float fireRate;
    private float fireCountDown;
    public float DamagePerFire;
    public float range;
    

    /*
     * Rotation
     */
    [Header("Structural data for rotation")]
    [SerializeField]
    private GameObject baseStructure;
    private Transform rotatingStructure;
    [SerializeField]
    private List <GameObject> waypons;
    public float turnSpeed = 10;

    /*
     * Targeting system
     */
    private GameObject target = null;
    private Spawn_manager enemyGenerator;//spawnmanager

    /*
     * Upgrading
     */
    [Header("Upgrade")]
    public GameObject upgradeTo = null;
    public int upgradeCost;

    // Start is called before the first frame update
    void Start()
    {
        //get access to spawnmanager
        enemyGenerator = GameObject.Find("enemies_holder").GetComponent<Spawn_manager>();   
        //start targeting
        InvokeRepeating("updateTarget", 0f, 0.5f);
        //setup rotating parts
        rotatingStructure = baseStructure.transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        //no target, no chocolate
        if (target == null)
            return;

        //Facing Enemy
        rotating();

        //FireEnemyAtFixInterval
        if (fireCountDown <= 0)
        {
            fire();
            fireCountDown = 1/fireRate;
        }
        else fireCountDown -= Time.deltaTime;

    }

    void rotating()
    {
        //Yeayyy Targets
        //Debug.Log("found");
        //where are you ?
        Vector3 direction = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //Let me some time to face you... 
        Vector3 rotation = Quaternion.Lerp(rotatingStructure.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        rotatingStructure.rotation = Quaternion.Euler(0f, rotation.y, 0);

        //same but rising the waypons in direction of the entity
        foreach (GameObject waypon in waypons)
        {
            waypon.transform.rotation = Quaternion.Euler(0, 0, rotation.z);
        }
        /**/
    }

    void fire()
    {
        Debug.Log("Peww");
    }

    GameObject getClosestEnemy(List<GameObject> enemies)
    {
        //on initie notre filtre avec un objet null infiniment loin
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach(GameObject enemy in enemies)
        {
            Vector3 directionToTarget = enemy.transform.position - transform.position;
            float distanceSqrToTarget = directionToTarget.sqrMagnitude;

            //Si un objet est plus proche que le précédent, alors on le prend comme nouvelle target
            if(distanceSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqrToTarget;
                closest = enemy;
            }
        }

        return closest;
    }

    void updateTarget()
    {
        if (enemyGenerator.aliveEnemies.Count != 0)
        {
            //on prend le plus proche des enemis en vie
            target = getClosestEnemy(enemyGenerator.aliveEnemies);

        }

        //si l'enemy visé est trop loin, alors on ne le prend pas comme target.
        if (target && Vector3.Distance(target.transform.position, transform.position) > range)
            target = null;


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
