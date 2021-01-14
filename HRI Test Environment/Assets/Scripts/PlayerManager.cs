using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string filename, path;
    RecordMovement timeline;
    // Start is called before the first frame update
    private void Awake()
    {
        path += filename;
        timeline = GameObject.FindGameObjectWithTag("Human").GetComponent<RecordMovement>();
    }
    void Start()
    {
        timeline.Begin();
    }
    void End()
    {
        using (FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write))
        {
            using(StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine(timeline.End());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ended recording");
            End();
        }
    }
}
