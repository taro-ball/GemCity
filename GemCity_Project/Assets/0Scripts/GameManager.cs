//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameOver;
    public static float globalDamage = 1f;
    public static float globalRate = 1;
    public static float globalRange = 1;
    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public Text moneyTotalText;
    public Text enemiesTotalText;
    public Text wavesText;
    public Text moneyWinTotalText;
    public Text enemiesWinTotalText;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start before: Time.timeScale: " + Time.timeScale);
        Time.timeScale = 1f;
        //Debug.Log("Start after: Time.timeScale: " + Time.timeScale);
        gameOver = false;
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
            return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            WinLevel();
        }
        if (PlayerStats.Lives <= 0 && !gameOver)
        {
            EndGame();
        }
    }
    void EndGame()
    {
        //Debug.Log("Dead!!!!!!!!!!!!!!!!!!!!!!!");
        gameOver = true;
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);

        moneyTotalText.text = PlayerStats.moneyTotal.ToString();
        enemiesTotalText.text = PlayerStats.enemiesTotal.ToString();
        wavesText.text = PlayerStats.waves.ToString();
    }
    public void WinLevel()
    {
        //Debug.Log("WON!!");
        completeLevelUI.SetActive(true);
        moneyWinTotalText.text = PlayerStats.moneyTotal.ToString();
        enemiesWinTotalText.text = PlayerStats.enemiesTotal.ToString();
        gameOver = true;
    }
}
