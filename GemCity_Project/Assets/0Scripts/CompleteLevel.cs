using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    public string nextLevel = "Level2";
    public int levelToReach = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Next() {
        PlayerPrefs.SetInt("levelReached", levelToReach);
        SceneManager.LoadScene("LevelSelect");
    }
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
