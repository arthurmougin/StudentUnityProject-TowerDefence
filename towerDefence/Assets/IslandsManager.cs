using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IslandsManager : MonoBehaviour
{
    private bool lookingToAddIsland = false;
    public GameObject CollisionBox;
    private bool activeCollBox = false;
    public GameObject[] island_prefabs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //On regarde ou pointe le joueur tout le temps
        

        //S'il doit ajouter une tour
        if (lookingToAddIsland)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit  hit;
            //Alors on regarde ou il pointe sur le navmesh, s'il pointe sur le navmesh
            if (Physics.Raycast(ray,out hit))
            {
                CollisionBox.SetActive(true);
                activeCollBox = true;
                CollisionBox.transform.position = hit.point;
                // Debug.Log(hit.collider.gameObject.name);

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject babyIsland = island_prefabs[Random.Range(0, island_prefabs.Length - 1)];
                    Vector3 rotationRand = new Vector3(0, Random.Range(0, 360), 0);
                    Instantiate(babyIsland, hit.point,Quaternion.Euler(rotationRand),transform);
                    onAddIsland();
                }
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
        lookingToAddIsland = !lookingToAddIsland;
        if (!lookingToAddIsland)
        {
            CollisionBox.SetActive(false);
            activeCollBox = false;
        }
        else
        {
            CollisionBox.SetActive(true);
            activeCollBox = true;
        }
    }
}
