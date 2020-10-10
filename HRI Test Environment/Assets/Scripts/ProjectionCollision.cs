using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionCollision : MonoBehaviour
{
    public GameObject robot, human;
    // Start is called before the first frame update
    void Start()
    {
        robot = GameObject.FindGameObjectWithTag("Robot");
        human = GameObject.FindGameObjectWithTag("Human");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Human")
        {
            bool dirToFace = Vector3.SignedAngle(robot.transform.position, human.transform.position, Vector3.up) > 0;
            robot.GetComponent<AdvancedCollect>().obstructed = true;
            robot.GetComponent<AdvancedCollect>().obstructedPath(transform.position, dirToFace);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Human")
        {
            robot.GetComponent<AdvancedCollect>().obstructed = false;
            robot.GetComponent<AdvancedCollect>().tooClose = false;
        }
    }
}
