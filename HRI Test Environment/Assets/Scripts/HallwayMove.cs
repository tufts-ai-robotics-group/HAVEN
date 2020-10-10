using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HallwayMove : MonoBehaviour
{
    public GameObject goal;
    public NavMeshAgent robot;

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Human");
        robot = GetComponent<NavMeshAgent>();
        robot.destination = goal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
