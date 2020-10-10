using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("main_menu");
        }
    }

    public void Hallway_Scene()
    {
        SceneManager.LoadScene("hallway_scene");
    }

    public void Room_Scene()
    {
        SceneManager.LoadScene("room_scene");
    }

    public void Menu_Scene()
    {
        SceneManager.LoadScene("main_menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

}
