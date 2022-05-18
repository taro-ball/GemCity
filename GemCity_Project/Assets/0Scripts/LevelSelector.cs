using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    // Start is called before the first frame update
    private void Start()
    {
        int LevelReached = PlayerPrefs.GetInt("levelReached", 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i+1 > LevelReached)
            {
                levelButtons[i].interactable = false;
            }
            
        }
    }
    public void SelectLevel(string levelName)
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<MusicClass>().StopMusic();
        SceneManager.LoadScene(levelName);
    }
}
