using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moveto : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject goal;
    private GameObject LocalPositionner;
    private NavMeshAgent navAgent;
    private float pathLength = 0f;

    public float mediumLeapHeight = 0f;
    public float maxDeltaLeapHeight = 10f;

    private float LeapHeight = 0f;

    public float localLeapHeight = 0f;

    void Start()
    {

        /* Setup navigation */
        navAgent = GetComponent<NavMeshAgent>();
        goal = GameObject.Find("navmesh_holder/navmesh_Goal");
        navAgent.destination = goal.transform.position;

        /* Mise en place de la trajectoire courbée sur l'axe y */
        LocalPositionner = this.gameObject.transform.GetChild(0).gameObject;
        //on cherche aléatoirement la taille de la courbe dans l'interval donné en paramêtre
        LeapHeight = Random.Range((mediumLeapHeight - maxDeltaLeapHeight), (mediumLeapHeight + maxDeltaLeapHeight));
        localLeapHeight = LeapHeight;

    }

    // Update is called once per frame
    void Update()
    {


        // gestion de fin de parcour
        if (navAgent.remainingDistance < 5 && navAgent.remainingDistance > 0)
        {
            //TODO remove points to goal
            Destroy(gameObject);
        }
        else
        { // gestion de la variation d'altitude 


            if (pathLength == 0 || pathLength == Mathf.Infinity)// Le calcul du path prend un peu de temps, on l'attend donc pour éviter une catastrophe
            {
                if(navAgent.remainingDistance != 0)
                    pathLength = navAgent.remainingDistance;
                return;
            }



            Debug.Log(localLeapHeight);
            LocalPositionner.transform.position.Set(0, localLeapHeight, 0);
        }
    }
}
