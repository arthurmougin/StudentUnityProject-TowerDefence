
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    private int impact = 1;
    public float speed = 70f;

    public void Setup(Transform _target, int _impact)
    {
        target = _target;
        impact = _impact;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.GetChild(0).GetChild(0).position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;
        if(direction.magnitude <= distancePerFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distancePerFrame,Space.World);

    }
    

    void HitTarget()
    {

        Debug.Log("touché");
        Destroy(gameObject);
        target.gameObject.GetComponent<Moveto>().Touché(impact);
        return;
    }
}

