using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moveto : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject goal;


    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.destination = goal.transform.position;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
