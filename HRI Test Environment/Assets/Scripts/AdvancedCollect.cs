using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;
public class AdvancedCollect : MonoBehaviour
{
    // Start is called before the first frame update
    const float DIAMETER = 1f, CURVE_ANGLE = 3f, CURVE_DIAMETER = .3f, AVOID_ANGLE = 60f;
    const int PER_PATH = 100;
    public Battery battery;
    public GameObject[] collectibles, markers, curveMarkers;
    public GameObject marker, curveMarker, lArrow, rArrow;
    public Vector3 initPos;
    public Vector3[] positions, sequence, curvePath;
    private int destPoint, pathProgress, curveNum;
    private float initAngle, destAngle, rotSpeed;
    private NavMeshAgent agent;
    public bool obstructed, tooClose;
    private bool facing, targetAcquired, plotted, curvePlotted;
    void Start()
    {
        battery = GetComponent<Battery>();
        obstructed = false;
        facing = false;
        targetAcquired = false;
        rotSpeed = 50f;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        collectibles = GameObject.FindGameObjectsWithTag("R_Collectible");
        positions = new Vector3[collectibles.Length];
        bool[] used = new bool[collectibles.Length];
        for (int i = 0; i < collectibles.Length; i++)
        {
            int nextPos = (int) Random.Range(0f, used.Length - 1);
            while (used[nextPos])
            {
                nextPos = (int)Random.Range(0f, used.Length - 1);
            }
            positions[nextPos] = collectibles[i].transform.position;
            used[nextPos] = true;
        }
        lArrow.SetActive(false);
        rArrow.SetActive(false);
        destPoint = -1;
        pathProgress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!battery.charging)
        {
            if (!obstructed)
            {
                unobstructedUpdate();
            }
            else
            {
                if (tooClose)
                {
                    agent.isStopped = true;
                }
                else
                {
                    facing = false;
                    //true = right, false = left

                }
                //TODO: figure out how obstruction works
            }
        }
    }
    void unobstructedUpdate()
    {
        if (!targetAcquired)
        {
            acquireTarget();
            initAngle = Vector3.SignedAngle(this.transform.forward, positions[destPoint] - transform.position, Vector3.up);
        }
        if (facing || curvePlotted)
            GoToNextPoint();
        else
        {
            Vector3 destVector = positions[destPoint] - transform.position;
            destAngle = Vector3.SignedAngle(this.transform.forward, positions[destPoint] - transform.position, Vector3.up);
            if ((initAngle > -60f && initAngle < 60f) && Vector3.Distance(transform.position, destVector) > 30)
            {
                noRotate(destAngle);
            }
            else
            {
                facing = rotation(destAngle);
            }
        }
    }
    void acquireTarget()
    {
        agent.ResetPath();
        if (destPoint == positions.Length - 1) return;
        destPoint++;
        if (destPoint < positions.Length) battery.NeedToGoHome(positions[destPoint]);
        targetAcquired = true;
    }

    void GoToNextPoint()
    {
        if (destPoint >= positions.Length)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        if (!plotted && !curvePlotted)
        {
            plotPath(transform.position, positions[destPoint], null);
        }
        else
        {
            
            if(pathProgress == sequence.Length)
            {
                agent.destination = positions[destPoint];
                if (Vector3.Distance(transform.position, positions[destPoint]) < 0.5f)
                {
                    targetAcquired = false;
                    facing = false;
                    plotted = false;
                    curvePlotted = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, sequence[pathProgress]) < 0.5f)
                {
                    Destroy(markers[pathProgress]);
                    pathProgress++;
                    updateArrows();
                    if (pathProgress != sequence.Length) agent.destination = sequence[pathProgress];
                    progressPath();
                }

            }

        }
        
    }

    void updateArrows()
    {
        float directional = Vector3.SignedAngle(this.transform.forward, positions[destPoint] - transform.position, Vector3.up);
        if (pathProgress == 0 || pathProgress >= sequence.Length) directional = Vector3.SignedAngle(this.transform.forward, positions[destPoint] - transform.position, Vector3.up);
        else directional = Vector3.SignedAngle(this.transform.forward, sequence[pathProgress] - transform.position, Vector3.up);
        Debug.Log(directional);
        if (directional >= 11 || directional <= -11)
        {
            if(directional > 0)
            {
                lArrow.SetActive(true);
                rArrow.SetActive(false);
            }
            else
            {
                rArrow.SetActive(true);
                lArrow.SetActive(false);
            }
        }
        else
        {
            lArrow.SetActive(false);
            rArrow.SetActive(false);
        }
    }
    bool rotation(float angle)
    {
        updateArrows();
        if (Mathf.Abs(destAngle) < 5) return true;
        float preAngle, angleChange;
        angleChange = rotSpeed * Time.deltaTime;
        if (angle < 0) angleChange *= -1;
        preAngle = angle;
        angle -= angleChange;
        this.transform.Rotate(0, angleChange, 0);
        return false;
    }
    void noRotate(float angle)
    {
        curveNum = (int) Mathf.Floor(Mathf.Abs(angle) / CURVE_ANGLE);
        curvePath = new Vector3[curveNum];
        curveMarkers = new GameObject[curveNum];
        int sign = 1;
        if (angle < 0) sign = -1;
        for(int i = 0; i < curveNum; i++)
        {
            Quaternion nextAngle = Quaternion.Euler(0, CURVE_ANGLE * (i+1) * sign, 0);
            Vector3 nextToFace = (nextAngle * transform.forward);
            curvePath[i] = nextToFace * (CURVE_DIAMETER * (i + 1)) + transform.position;
            curveMarkers[i] = Instantiate(curveMarker);
            curveMarkers[i].transform.position = curvePath[i];
        }
        plotPath(curvePath[curveNum - 1], positions[destPoint], curvePath);
        curvePlotted = true;
    }
    void plotPath(Vector3 startPoint, Vector3 endPoint, Vector3 [] curvePath)
    {
        float distance = Vector3.Distance(endPoint, startPoint);
        sequence = new Vector3[(int) Mathf.Ceil(distance / DIAMETER)];
        markers = new GameObject[(int)Mathf.Ceil(distance / DIAMETER)];
        sequence[0] = startPoint;
        Vector3 direction = (endPoint - startPoint) / distance; 
        for (int i = 1; i < sequence.Length; i++)
        {
            sequence[i] = (direction * (DIAMETER * i)) + startPoint;
            markers[i] = Instantiate(marker);
            markers[i].transform.position = sequence[i];
            markers[i].SetActive(false);
        }
        if (curvePath != null && curvePath.Length != 0)
        {
            Vector3 [] fullPath = new Vector3[sequence.Length + curvePath.Length];
            GameObject[] allMarkers = new GameObject[markers.Length + curveMarkers.Length];
            for (int i = 0; i < curvePath.Length; i++)
            {
                fullPath[i] = curvePath[i];
                allMarkers[i] = curveMarkers[i];
            }
            for (int i = 0; i < sequence.Length; i++)
            {
                fullPath[i + curvePath.Length] = sequence[i];
                allMarkers[i + curveMarkers.Length] = markers[i];
            }
            sequence = fullPath;
            markers = allMarkers;
        }
        plotted = true;
        pathProgress = 0;
    }
    void progressPath()
    {
        int toProject = pathProgress + PER_PATH;
        if (markers.Length < toProject) toProject = markers.Length;
        for(int i = pathProgress; i < toProject; i++)
        {
            if (i >= markers.Length || markers[i] == null) continue;
            markers[i].SetActive(true);
        }
    }
    public void obstructedPath(Vector3 hitMarker, bool dirToFace)
    {
        GameObject [] avoidMarkers, newMarkers;
        Vector3[] avoidPath, newPath;
        Vector3 peakDetour, startAvoiding = new Vector3(0,0,0), stopAvoiding = new Vector3(0,0,0), direction;
        int at, range, avoidCurveNum;
        if(Vector3.Distance(hitMarker, positions[destPoint]) < 2f || Vector3.Distance(hitMarker, transform.position) < 2f)
        {
            //Too close defined as two sphere lengths away from goal or robot
            tooClose = true;
        }
        else
        {
            float angToAvoid = AVOID_ANGLE;
            if (!dirToFace) angToAvoid *= -1;
            Quaternion avoidDir = Quaternion.Euler(0, angToAvoid, 0);
            direction = (hitMarker - transform.position) / Vector3.Distance(hitMarker, transform.position);
            //TODO: Arc movement around obstacle
            range = 2;
            at = detourMarks(hitMarker, ref startAvoiding, ref stopAvoiding, range);
            avoidCurveNum = (int)Mathf.Floor(Mathf.Abs(AVOID_ANGLE / CURVE_ANGLE));
            avoidPath = new Vector3[avoidCurveNum];
            avoidMarkers = new GameObject[avoidCurveNum];
            newPath = new Vector3[(sequence.Length - pathProgress) + avoidMarkers.Length];
            newMarkers = new GameObject[(sequence.Length - pathProgress) + avoidMarkers.Length];
            //building the half-ellipsoid arc
            for (int i = 0; i < avoidCurveNum; i++)
            {
                Quaternion nextAngle = Quaternion.Euler(0, angToAvoid - (CURVE_ANGLE * (i + 1)), 0);
                Vector3 nextToFace = (nextAngle * transform.forward);
                avoidPath[i] = nextToFace * (CURVE_DIAMETER * (i + 1)) + transform.position;
                avoidMarkers[i] = Instantiate(curveMarker);
                avoidMarkers[i].transform.position = avoidPath[i];
            }
            //change the path
            for(int i = pathProgress; i < at - range; i++)
            {
                newPath[i] = sequence[i];
                newMarkers[i] = markers[i];
            }
            for(int i = 0; i < avoidPath.Length; i++)
            {
                newPath[i + (at - range)] = avoidPath[i];
                newMarkers[i + (at - range)] = avoidMarkers[i];
            }
            for (int i = at + range; i < sequence.Length; i++)
            {
                newPath[i + (at - range) + avoidPath.Length] = sequence[i];
                newMarkers[i + (at - range) + avoidPath.Length] = markers[i];
            }
            sequence = newPath;
            markers = newMarkers;
            obstructed = false;
        }
    }
    int detourMarks(Vector3 hit, ref Vector3 start, ref Vector3 end, int howFar)
    {
        int at = 0;
        for (int i = 0; i < sequence.Length; i++)
        {
            if(sequence[i] == hit)
            {
                at = i;
                start = sequence[i - howFar];
                end = sequence[i + howFar];
                break;
            }
        }
        return at;
    }

    public void resumePath()
    {

    } 

    public void batteryDetour()
    {
        Vector3[] addDetour = new Vector3[positions.Length + 1];
        for (int i = destPoint + 1; i < destPoint; i++)
        {
            addDetour[i] = positions[i - 1];
        }
        addDetour[destPoint] = battery.home.transform.position;
        positions = addDetour;
        targetAcquired = false;
    }
}
