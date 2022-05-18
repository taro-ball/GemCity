using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject ui;

    //public string menuSceneName = "MainMenu";

    //public SceneFader sceneFader;
    private void Start()
    {
        Toggle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            
        }
        Debug.Log("Toggle: Time.timeScale: " + Time.timeScale);
    }

    public void Retry()
    {
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        Debug.Log("Retry: Time.timeScale: " + Time.timeScale);
    }

    public void Menu()
    {
        //Toggle();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
