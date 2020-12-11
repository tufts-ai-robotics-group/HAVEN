using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class HallwayTraverse : MonoBehaviour
{
    public GameObject[] waypoints, avoidPoints;
    public GameObject robot, leftArrow, rightArrow, avoidPath, currAvoidPath;
    public GameObject human;
    public NavMeshAgent agent;
    public int currPoint, avoidProg;
    public bool facing, targetAcquired, targetedNext, first, avoiding;
    private float destAngle, rotSpeed;
    private RaycastHit vision;
    public float rayLength;

    // Start is called before the first frame update
    private void Awake()
    {
        human = GameObject.FindGameObjectWithTag("Human");
        robot = GameObject.FindGameObjectWithTag("Robot");
        agent = robot.GetComponent<NavMeshAgent>();
        leftArrow = GameObject.FindGameObjectWithTag("Left_Arrow");
        rightArrow = GameObject.FindGameObjectWithTag("Right_Arrow");
        avoidPoints = new GameObject[2];
    }
    void Start()
    {
        rayLength = 5f;
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
        Debug.Log(agent.destination);
        updateArrows();
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red, 0.5f);
        if (Physics.Raycast(transform.position, transform.forward * rayLength, out vision, rayLength))
        {
            if (vision.collider.name == "Human Agent") {
                Debug.Log("stinky human detected " + Vector3.Distance(waypoints[currPoint].transform.position, robot.transform.position) + " far away");
                if (Vector3.Distance(waypoints[currPoint].transform.position, robot.transform.position) > 4f) {
                    if (Vector3.Distance(robot.transform.position, human.transform.position) <= 7f && !avoiding) {
                        currAvoidPath = Instantiate(avoidPath);
                        Vector3 pathVector = new Vector3(human.transform.position.x, -1f, human.transform.position.z);
                        avoidPath.transform.position = pathVector;
                        Debug.Log("avoidPath be at: " + avoidPath.transform.position);
                        Debug.Log("Human bet at: " + human.transform.position);
                        avoidProg = 0;
                        avoidPoints[0] = GameObject.Find("AvoidPoint 2");
                        avoidPoints[1] = GameObject.Find("AvoidPoint 3");
                        avoiding = true;
                    }
                } else if (!agent.isStopped) {
                    agent.isStopped = true;
                }
            }
        } else {
            agent.isStopped = false;
        }
        if (avoiding) {
            agent.SetDestination(avoidPoints[avoidProg].transform.position);
            if (Vector3.Distance(avoidPoints[avoidProg].transform.position, robot.transform.position) < 1f) {
                if (avoidProg == 1)
                {
                    agent.SetDestination(waypoints[currPoint].transform.position);
                    avoiding = false;
                } else {
                    avoidProg++;
                }
            }   
        }
        if (!avoiding && Vector3.Distance(waypoints[currPoint].transform.position, robot.transform.position) < 1f)
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
        
        if (other.tag == "Human")
        {
            Debug.Log("Contact Made, failure");
            SceneManager.LoadScene("Hallway");
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

