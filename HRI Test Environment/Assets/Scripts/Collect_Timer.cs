using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collect_Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject timer;
    private Collider collectible;
    private bool collided;
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Collect_Timer");
        timer.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "H_Collectible")
        {
            collided = true;
            collectible = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "H_Collectible")
        {
            collided = false;
            collectible = null;
        }   
    }
    // Update is called once per frame
    void Update()
    {
        timer.SetActive(collided);
        if (collided && collectible != null)
        {
            float timeRemaining = collectible.GetComponent<Collectible>().collideTime - collectible.GetComponent<Collectible>().currTime;
            timer.GetComponent<Text>().text = "Time till collected: " + timeRemaining.ToString("0.00");
        }
        else
        {
            collided = false;
        }
    }
}
