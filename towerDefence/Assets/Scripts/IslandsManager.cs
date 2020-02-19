using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class IslandsManager : MonoBehaviour
{
    private bool lookingToAddIsland = false;
    public Button addIslandButton;
    public GameObject CollisionBox;
    private ColliderListener CollidingTest;
    private GameObject[] island_prefabs;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(CollisionBox.name);
        CollidingTest = CollisionBox.transform.GetChild(0).gameObject.GetComponent<ColliderListener>();
        island_prefabs = GameManager.instance.island_prefabs;
        //Debug.Log(CollidingTest != null);
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
                CollisionBox.transform.position = hit.point;
                // Debug.Log(hit.collider.gameObject.name);

                if (Input.GetMouseButtonDown(0) && CollidingTest.aviable == true)
                {
                    GameObject babyIsland = island_prefabs[Random.Range(0, island_prefabs.Length - 1)];
                    Vector3 rotationRand = new Vector3(0, Random.Range(0, 360), 0);
                    Instantiate(babyIsland, hit.point,Quaternion.Euler(rotationRand),transform);
                    GameManager.instance.money -= GameManager.instance.islandPrice;
                    onAddIsland();
                }
            }
            else
            {
                CollisionBox.SetActive(false);
            };
        }
    }

    public void onAddIsland()
    {
        Debug.Log("onAddIsland");
        lookingToAddIsland = !lookingToAddIsland;
        if (!lookingToAddIsland)
        {
            CollisionBox.SetActive(false);
        }
        else
        {
            CollisionBox.SetActive(true);
        }
    }

    public void updateUI(){
        if(GameManager.instance.money < GameManager.instance.islandPrice){
            addIslandButton.interactable = false;
        }
        else {
            addIslandButton.interactable = true;
        }
        
    }
}
