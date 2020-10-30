using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSight : MonoBehaviour
{
    private RaycastHit vision;
    public float rayLength;
    // Start is called before the first frame update
    void Start()
    {
        rayLength = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayLength, Color.red, 10f);
        Debug.Log(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * rayLength, out vision, rayLength));
        Debug.Log(vision.collider.name);
    }
}
