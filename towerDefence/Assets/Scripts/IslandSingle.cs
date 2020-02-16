using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSingle : MonoBehaviour
{

    public float hoverFactor = 1.5f;
    public UIHandler TourelleUIHolder;
    private bool isOwner = false;

    // Start is called before the first frame update
    void start(){
       
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
}
