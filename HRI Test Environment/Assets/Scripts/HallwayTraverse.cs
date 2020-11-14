using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class HallwayTraverse : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject robot, leftArrow, rightArrow;
    public NavMeshAgent agent;
    public int currPoint;
    public bool facing, targetAcquired, targetedNext, first;
    private float destAngle, rotSpeed;
    private RaycastHit vision;
    public float rayLength;

    // Start is called before the first frame update
    private void Awake()
    {
        robot = GameObject.FindGameObjectWithTag("Robot");
        agent = robot.GetComponent<NavMeshAgent>();
        leftArrow = GameObject.FindGameObjectWithTag("Left_Arrow");
        rightArrow = GameObject.FindGameObjectWithTag("Right_Arrow");
    }
    void Start()
    {
        rayLength = 10f;
        currPoint = 0;
        rotSpeed = 50f;
        facing = false;
        targetAcquired = false;
        targetedNext = false;
        first = true;
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateArrows();
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red, 0.5f);
        if (Physics.Raycast(transform.position, transform.forward * rayLength, out vision, rayLength))
        {
            if (vision.collider.name == "Human Agent")
            {
                if (Vector3.Distance(waypoints[currPoint].transform.position, robot.transform.position) > 6f)
                {

                }
            }
        }
        if (Vector3.Distance(waypoints[currPoint].transform.position, robot.transform.position) < 1f)
        {
            facing = false;
            targetAcquired = false;
            targetedNext = false;
        }
        if (!targetAcquired)
        {
            if (!targetedNext)
            {
                if (!first) currPoint++;
                first = false;
                if (currPoint == waypoints.Length) return;
                targetedNext = true;
            }
            else if (facing)
            {
                agent.SetDestination(waypoints[currPoint].transform.position);
                targetAcquired = true;
            }
            else
            {
                Vector3 destVector = waypoints[currPoint].transform.position - transform.position;
                destAngle = Vector3.SignedAngle(this.transform.forward, waypoints[currPoint].transform.position - transform.position, Vector3.up);
                facing = rotation(destAngle);
            }
        }
    }


    bool rotation(float angle)
    {
        if (Mathf.Abs(destAngle) < 5) return true;
        float preAngle, angleChange;
        angleChange = rotSpeed * Time.deltaTime;
        if (angle < 0) angleChange *= -1;
        preAngle = angle;
        angle -= angleChange;
        this.transform.Rotate(0, angleChange, 0);
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Contact Made, failure");
        if (other.tag == "Human")
        {
            SceneManager.LoadScene("hallway_scene");
        }
    }
    void updateArrows()
    {
        float directional = Vector3.SignedAngle(this.transform.forward, waypoints[currPoint].transform.position - transform.position, Vector3.up);
        //Debug.Log(directional);
        if (directional >= 11 || directional <= -11)
        {
            if (directional > 0)
            {
                leftArrow.SetActive(true);
                rightArrow.SetActive(false);
            }
            else
            {
                rightArrow.SetActive(true);
                leftArrow.SetActive(false);
            }
        }
        else
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
        }
    }
}

