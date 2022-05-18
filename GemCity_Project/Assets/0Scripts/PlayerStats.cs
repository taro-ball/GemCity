using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public static int Money;
    public int startMoney = 350;

    public static int Lives;
    public int startLives = 20;

    public static int waves;
    public static int moneyTotal;
    public static int enemiesTotal;
    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
        waves = 0;
        moneyTotal = 0;
        enemiesTotal = 0;
    }
}
