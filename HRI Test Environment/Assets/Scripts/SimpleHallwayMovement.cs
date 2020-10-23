using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleHallwayMovement : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject robot, leftArrow;
    public NavMeshAgent agent;
    public int currWaypoint;
    // Start is called before the first frame update
    private void Awake()
    {
        leftArrow = GameObject.FindGameObjectWithTag("Left_Arrow");
        robot = GameObject.FindGameObjectWithTag("Robot");
    }
    void Start()
    {
        Debug.Log(waypoints[0]);
        Debug.Log(waypoints[1]);
        Debug.Log(waypoints[2]);
        Debug.Log(waypoints[3]);
        currWaypoint = 0;
        agent = robot.GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[0].transform.position);
        leftArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currWaypoint);
        if (Vector3.Distance(robot.transform.position, waypoints[currWaypoint].transform.position) < 0.5)
        {
            
            agent.ResetPath();
            currWaypoint++;
            agent.SetDestination(waypoints[currWaypoint].transform.position);
            if(currWaypoint == 1)
            {
                leftArrow.SetActive(true);
            }
            else
            {
                leftArrow.SetActive(false);
            }

        } 
    }
}
