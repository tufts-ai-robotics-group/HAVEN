using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    // Start is called before the first frame update
    const float FULL_LEVEL = 100f, ENOUGH_JUICE = 0.5f, RATE = 0.1f;
    public GameObject home, levelIndicator, robot;
    public Sprite[] levelSprites;
    private float currLevel, distFromHome;
    public bool hardMode, goingHome, charging;
    
    void Start()
    {
        robot = GameObject.FindGameObjectWithTag("Robot");
        home = GameObject.FindGameObjectWithTag("Charging Base");
        currLevel = FULL_LEVEL;
    }

    // Update is called once per frame
    void Update()
    {
        updateVisual();
        distFromHome = Vector3.Distance(this.transform.position, home.transform.position);
        if (distFromHome < 0.75f && goingHome)
        {
            if (currLevel >= FULL_LEVEL)
            {
                charging = true;
                currLevel += RATE;
            }
            else
            {
                charging = false;
                goingHome = false;
                robot.GetComponent<AdvancedCollect>().resumePath();
            }
        }
        else if(hardMode && distFromHome / currLevel < ENOUGH_JUICE)
        {
            robot.GetComponent<AdvancedCollect>().batteryDetour();
        }
        else
        {
            currLevel -= Time.deltaTime;
        }
    }
    public void NeedToGoHome(Vector3 nextDest)
    {
        if (Vector3.Distance(robot.transform.position, nextDest) + Vector3.Distance(home.transform.position, nextDest) >= currLevel)
        {
            goingHome = true;
            robot.GetComponent<AdvancedCollect>().batteryDetour();
        }
    }
    void updateVisual() 
    { 
        if (currLevel > 80)
        {
            levelIndicator.GetComponent<SpriteRenderer>().sprite = levelSprites[0];
        }
        else if (currLevel > 60)
        {
            levelIndicator.GetComponent<SpriteRenderer>().sprite = levelSprites[1];
        }
        else if (currLevel > 40)
        {
            levelIndicator.GetComponent<SpriteRenderer>().sprite = levelSprites[2];
        }
        else if (currLevel > 20)
        {
            levelIndicator.GetComponent<SpriteRenderer>().sprite = levelSprites[3];
        }
        else
        {
            levelIndicator.GetComponent<SpriteRenderer>().sprite = levelSprites[4];
        }
    }
}
