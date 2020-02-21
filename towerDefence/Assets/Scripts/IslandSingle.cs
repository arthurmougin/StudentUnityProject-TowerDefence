using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSingle : MonoBehaviour
{

    public float hoverFactor = 1.5f;

    /*
    For strange reasons, the prefab can't save the UIhandler 
    when set in public and the islandprefab is deleted from 
    the scene

    as a quickfix, i take it from the gameManager who is a trust source
    */
    private UIHandler TourelleUIHolder;
    private bool isOwner = false;

    // Start is called before the first frame update
    void Start(){

        TourelleUIHolder = GameManager.instance.uIHandler;
       
    }

     // Update is called once per frame
    void Update()
    {

        /*
            The UI is based on Ownership of the UI handler,
            The Island who own it is allowed to place the UI on the screen relative to itself.
        */
        if(isOwner){
            Vector3 UIPos = Camera.main.WorldToScreenPoint(this.transform.position);

            /**
            Strange Bug, UIPos can have a z value that make it invisible when the 
            "this.transform.position" is too far from the camera.

            This line (UIPos.z = 0;) fix everything
            */
            UIPos.z = 0;
            //Debug.Log(UIPos.ToString());
            
            TourelleUIHolder.transform.position = UIPos;
        }
    }


/*
For esthetical reason i did'nt wanted to change the colors of the islands to show focus
soo i went for scale changing. Dont worrie, it only apply to the island and not to 
the tower, this way it can't impact the gameplay
*/
    void OnMouseEnter() {
        transform.localScale = transform.localScale* hoverFactor;
    }

    void OnMouseExit() {
        transform.localScale = transform.localScale / hoverFactor;
    }

    void OnMouseDown(){
        /*
        For safety reasons, One island can't decide to toggle the UI handler or not
        sooo it have to be polite and ask.
        */
        if(isOwner)
        {
            TourelleUIHolder.dismissOwnerShip(gameObject);
        }else {
            TourelleUIHolder.askOwnerShip(gameObject);
        }
    }

    /*The UI handler may decide to accept it's request and give or remove it's ownership */
    public void setOwnerShip(bool owning){
        isOwner = owning;
    }

    /*
    sometimes the UI handler may need to get the tower attached to this island
    (attached by being brothers and having one same Island_holder as parent)
    */
    public GameObject askTower(){
        if(transform.parent.childCount > 1)
            return transform.parent.GetChild(1).gameObject;
        else return null;
    }
}
