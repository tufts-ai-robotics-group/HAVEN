using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class Snaps{
    public float elapsed;
    public Vector3 currPos;
    public Quaternion currRot;
    public Snaps(float _elapsed, Vector3 _currPos, Quaternion _currRot)
    {
        elapsed = _elapsed;
        currPos = _currPos;
        currRot = _currRot;
    }
    public override string ToString()
    {
        return "Elapsed: " + elapsed + ",Position: " + currPos + ",Rotation: " + currRot;
    }
}

public class RecordMovement : MonoBehaviour
{
    private GameObject player;
    private bool rec;
    private float interval, elapsed;
    Queue<Snaps> timelapse;
    
    // Start is called before the first frame update
    void Awake()
    {
        rec = false;
        interval = 1f;
        elapsed = 0f;
        player = GameObject.FindGameObjectWithTag("Human");
        timelapse = new Queue<Snaps>();
    }
    private void Start()
    {
        RecordStep();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rec)
        {
            if (Time.time > elapsed)
            {
                RecordStep();
            }
        }
    }
    public void Begin()
    {
        rec = true;
    }
    
    public void RecordStep()
    {
        Snaps snap = new Snaps(elapsed, player.transform.position, player.transform.rotation);
        timelapse.Enqueue(snap);
        elapsed += interval;
        Debug.Log("Recorded Step: " + snap.ToString());
    }
    public string End()
    {
        rec = false;
        return Save();
    }
    public string Save()
    {
        Snaps[] timeline = timelapse.ToArray();
        return JsonHelper.ToJson(timeline);
    }
}
