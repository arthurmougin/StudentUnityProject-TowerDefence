﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moveto : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject goal;
    private GameObject navGoal;
    private GameObject LocalPositionner;
    private NavMeshAgent navAgent;
    private float pathLength = 0f;
    private float LeapHeight = 0f;

    public float mediumLeapHeight = 0f;
    public float maxDeltaLeapHeight = 10f;
    private float localLeapHeight = 0f;
    private float previousLocalLeapHeight = 0f;
    private float remainingDist = 100f;
    private float position = 0;

    public float value = 2f;

    void Start()
    {
        /* Setup navigation */
        navAgent = GetComponent<NavMeshAgent>();
        navGoal = GameObject.Find("navmesh_holder/navmesh_Goal");
        goal = GameObject.Find("goal_island_prefab");
        navAgent.destination = navGoal.transform.position;

        /* Mise en place de la trajectoire courbée sur l'axe y */
        LocalPositionner = this.gameObject.transform.GetChild(0).gameObject;

        //on cherche aléatoirement la taille de la courbe dans l'interval donné en paramêtre
        LeapHeight = Random.Range((mediumLeapHeight - maxDeltaLeapHeight), (mediumLeapHeight + maxDeltaLeapHeight));
    }

    // Update is called once per frame
    void Update()
    {
        //calcul de la distance entre la position actuelle et la destination (on suppose une trajectoire en ligne droite, l'evitement d'obstacle n'est pas génant)
        remainingDist = (navAgent.destination - gameObject.transform.position).magnitude;
        position = pathLength - remainingDist;

        if (remainingDist < 100 && remainingDist > 0)
        {
           // Debug.Log("damagingDefences");
            //when an enemy arrive to spawn it damages it proportionnally to it's remainings shards
            goal.GetComponent<Goal>().OnTakeDamage(transform.GetChild(0).GetChild(0).childCount+1);

            //and then self destruct
            SelfDestroy();
        }
        else
        {
            updateHeight();
        }
        
    }

    void updateHeight()
    {
        // gestion de la variation d'altitude 
        if (pathLength == 0)// on update la distance à parcourir en fonction du premier calcul de distance etabli
        {
            pathLength = remainingDist;
            position = pathLength - remainingDist;
        }

        localLeapHeight = LeapHeight;
        /*
         * On cherche à reproduire une trajectoire en cloche, ou un U en fonction du signe de LeapHeight.
         * 
         * On utilise la courbe de sinus sur l'interval [0,Pi] (https://en.wikipedia.org/wiki/Trigonometry)
         * 
         * Pour rester sur cette courbe, on calcul le produit en croix entre l'avancement de l'entité 
         * jusqu'a l'objectif et l'avancement sur l'interval Pi. 
         *  
         *     Mathf.Sin( ( position * Mathf.PI) / pathLength ) 
         *      
         * On a donc la bonne courbe mais pas la bonne magnitude (sinus retourne une valeur entre 1 et 0 dans 
         * l'interval [0,Pi]), il faut pour cela  emplifier sa taille en multipliant le tout avec LeapHeight;
         * 
         *      Mathf.Sin( ( position * Mathf.PI) / pathLength ) * LeapHeight
         *      
         * On applique le resultat à l'altitude à l'instant T (localLeapHeight) et on fait un translate de 
         * l'écart entre la position actuelle et la précédente position
         */

        localLeapHeight = Mathf.Sin((position * Mathf.PI) / pathLength) * LeapHeight;

        LocalPositionner.transform.Translate(0, localLeapHeight - previousLocalLeapHeight, 0);

        previousLocalLeapHeight = localLeapHeight;
    }

    void SelfDestroy()
    {
        //Debug.Log("boom");
        Destroy(gameObject);
    }

    public void Touché(int impact)
    {
        //TODO particule system

        int childCount = transform.GetChild(0).GetChild(0).childCount;
        //Debug.Log(childCount + " " + impact);
        if (childCount < impact)
        {   
            GameManager.instance.money += value;
            //Debug.Log("Destruction");
            SelfDestroy();
            return;
        }
        //Debug.Log("Impact");

        while (impact > 0)
        {
            impact--;
            int destroyedIndex = Random.Range(0, (childCount - 1));
            Destroy(transform.GetChild(0).GetChild(0).GetChild(destroyedIndex).gameObject);
        }
    }
}
