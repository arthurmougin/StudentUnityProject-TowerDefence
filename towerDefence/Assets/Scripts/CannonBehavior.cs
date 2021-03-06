﻿using System.Collections;
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
    public int DamagePerFire;
    public float range;
    public GameObject bullet;
    public Transform firepoint;
   
    

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

    /*
     * Upgrading
     */
    [Header("Upgrade")]
    public GameObject upgradeTo = null;
    public int cost;

    /*
     * Animation
     */
    private Animator anim;
    private int firingHash = Animator.StringToHash("firing");
    private bool animated = false;

    // Start is called before the first frame update
    void Start()
    {
        //start targeting
        InvokeRepeating("updateTarget", 0f, 0.5f);
        //setup rotating parts
        rotatingStructure = baseStructure.transform.GetChild(0);
        anim = transform.GetChild(0).GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //no target, no chocolate
        if (target == null)
        {
            if (animated)
            {
                animated = false;
                anim.SetBool("firing", animated);
                Debug.Log(animated);
            }
            return;
        }

        if (!animated)
        {
            animated = true;
            anim.SetBool("firing", animated);
            Debug.Log(animated);
        }
        

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
       // Debug.Log("Peww");
        GameObject bulletTmp = (GameObject) Instantiate(bullet,firepoint.position,firepoint.rotation);
        Bullet bulletScript = bulletTmp.GetComponent<Bullet>();
        if(bulletScript != null)
        {
            bulletScript.Setup(target.transform, DamagePerFire);
        }
    }

    GameObject getClosestEnemy(List<GameObject> enemies)
    {
        //on initie notre filtre avec un objet null infiniment loin
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach(GameObject enemy in enemies)
        {
            if (enemy)
            {
                Vector3 directionToTarget = enemy.transform.position - transform.position;
                float distanceSqrToTarget = directionToTarget.sqrMagnitude;

                //Si un objet est plus proche que le précédent, alors on le prend comme nouvelle target
                if (distanceSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqrToTarget;
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    void updateTarget()
    {
        if (Spawn_manager.instance.aliveEnemiesCount() != 0)
        {
            //on prend le plus proche des enemis en vie
            target = getClosestEnemy(Spawn_manager.instance.GetAliveEnemies());

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
