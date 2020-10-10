using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataInput : MonoBehaviour
{
    public string username;
    public GameObject userInput;
    // Start is called before the first frame update
    void Start()
    {
        userInput.GetComponent<InputField>().Select();
        userInput.GetComponent<InputField>().ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setUsername()
    {
        UserData.username = userInput.GetComponent<InputField>().text;
        Debug.Log("Username is: " + UserData.username);
    }
}
