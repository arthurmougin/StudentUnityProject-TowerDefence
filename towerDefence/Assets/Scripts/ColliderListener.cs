using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ColliderListener : MonoBehaviour
{
    public bool aviable = true;
    private Material safe;
    [SerializeField]
    private Material warning;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        //Debug.Log(rend != null);
        safe = rend.material;
        aviable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "navmesh")
        {
            aviable = true;
            rend.material = safe;
            return;
        }
            
       // Debug.Log("triggerEnter");
        aviable = false;
        rend.material = warning;
    }
    void OnTriggerExit(Collider c)
    {
      //  Debug.Log("OnTriggerExit");
        aviable = true;
        rend.material = safe;
        
    }
    /*
    void OnTriggerStay(Collider c)
    {
       // Debug.Log("OnTriggerStay");
    }*/
}
