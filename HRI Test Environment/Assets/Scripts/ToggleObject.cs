using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject[] targets;
    public string targetTag, key;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject obj in targets)
        {
            obj.SetActive(active);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            active = !active;
            foreach(GameObject obj in targets)
            {
                obj.SetActive(active);
            }
        }
    }
}
