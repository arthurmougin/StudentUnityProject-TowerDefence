using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private IslandSingle Owner = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void askOwnerShip(GameObject caller){
        Debug.Log("askOwnerShip");
        //if someone ask for ownership while an other already own it, 
        //the new caller is the new owner.
        //We announce to the previous owner that he dont own us anymore
        if(Owner != null){
            Owner.setOwnerShip(false);
        }
        else {
            //if that's the first time, we display the true UI
            foreach(Transform child in transform){
                child.gameObject.SetActive(true);
            }
        }
        Owner = caller.GetComponent<IslandSingle>();
        Owner.setOwnerShip(true);

        
    }

    public void dismissOwnerShip(GameObject caller){
        Debug.Log("dismissOwnerShip");
        IslandSingle callingComponent = caller.GetComponent<IslandSingle>();
        //If the one asking for dismis ownership is the actual owner, let's go
        if(Owner == callingComponent){
            Owner.setOwnerShip(false);
            Owner = null;
            //We hide the UI
            foreach(Transform child in transform){
                child.gameObject.SetActive(false);
            }
        }
        else {
            //If a component asking for dismissing ownership, 
            //then it is a Bug and he shouldnt believe to be the owner
            Debug.Log("You are not the owner and cant ask for dismissingOwnership");
            callingComponent.setOwnerShip(false);
        }
    }
}
