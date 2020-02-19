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
        if(isOwner){
            Vector3 UIPos = Camera.main.WorldToScreenPoint(this.transform.position);
            TourelleUIHolder.transform.position = UIPos;
        }
    }

    void OnMouseEnter() {
        transform.localScale = transform.localScale* hoverFactor;
    }

    void OnMouseExit() {
        transform.localScale = transform.localScale / hoverFactor;
    }

    void OnMouseDown(){
        Debug.Log("OnMouseDown");
        Debug.Log(TourelleUIHolder != null);
        if(isOwner)
        {
            TourelleUIHolder.dismissOwnerShip(gameObject);
        }else {
            TourelleUIHolder.askOwnerShip(gameObject);
        }
    }

    public void setOwnerShip(bool owning){
        isOwner = owning;
    }

    public GameObject askTower(){
        if(transform.parent.childCount > 1)
            return transform.parent.GetChild(1).gameObject;
        else return null;
    }
}
