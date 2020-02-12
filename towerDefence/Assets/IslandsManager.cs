using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IslandsManager : MonoBehaviour
{
    private bool lookingToAddIsland = false;
    public GameObject CollisionBox;
    private bool activeCollBox = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //On regarde ou pointe le joueur tout le temps
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //S'il doit ajouter une tour
        if (lookingToAddIsland)
        {
           
            NavMeshHit hit;
            //Alors on regarde ou il pointe sur le navmesh, s'il pointe sur le navmesh
            if (NavMesh.Raycast(ray.origin, ray.GetPoint(100000), out hit, NavMesh.AllAreas))
            {
                CollisionBox.SetActive(true);
                activeCollBox = true;
                CollisionBox.transform.position = hit.position;

            }
            else
            {
                CollisionBox.SetActive(false);
                activeCollBox = false;
            };
        }
    }

    public void onAddIsland()
    {
        lookingToAddIsland = true;
    }
}
