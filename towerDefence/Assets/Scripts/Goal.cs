using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public float maxLive = 100;
    private float actualLive;
    public GameObject shield;
    public GameObject gameManager;
    public float minShieldSize;
    private float maxShieldSize;


    // Start is called before the first frame update
    void Start()
    {
        actualLive = maxLive;
        maxShieldSize = shield.transform.localScale.x;
    }

    public void reset()
    {
        actualLive = maxLive;
        changeSchieldScale();
    }

    // Update is called once per frame
    public void OnTakeDamage(int damage)
    {
        actualLive -= damage;
        Debug.Log(damage + " " +  actualLive);
        if (actualLive >= 0)//tant qu'il y a de la vie
        {
            //on montre notre faiblesse
            changeSchieldScale();
        }
        else //sinon c'est mort
        {
            gameManager.GetComponent<GameManager>().onShieldFallen();
        }
    }

    void changeSchieldScale()
    {
        float newScale = (actualLive * (maxShieldSize - minShieldSize) / maxLive) + minShieldSize;
        shield.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
