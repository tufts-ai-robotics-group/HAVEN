using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Collectible : MonoBehaviour
{

    public string type;
    public GameObject target;
    public float collideTime, currTime;
    public bool collided, human, isTarget;
    // Start is called before the first frame update
    void Start()
    {
        collided = false;
        human = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (collided)
        {
            currTime += Time.deltaTime;
            if(currTime >= collideTime)
            {
                target.GetComponent<Info>().collected += 1;
                Destroy(this.gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == type)
        {
            collided = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
      if(other.tag == type)
        {
            collided = false;
            currTime = 0;
        }
    }
}
