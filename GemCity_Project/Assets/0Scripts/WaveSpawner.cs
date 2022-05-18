using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class WaveSpawner : MonoBehaviour
{
    public static int enemiesAlive;
    //public Transform enemyPrefab;
    public Wave[] waves;
    public Transform spawnPoint;
    public float timeBetweenWaves = 1f;
    private float countdown = 2f;
    private int waveIndex = 0;
    public Text waveCountDownText;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            Debug.Log("Level finished!");
            gameManager.WinLevel();
            this.enabled = false;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }
        countdown -= Time.deltaTime; //reduce by 1 every second
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountDownText.text = string.Format("{0:00.0s}", countdown);
    }

    IEnumerator SpawnWave()
    {

        PlayerStats.waves++;

        Wave wave = waves[waveIndex];
        enemiesAlive = wave.count;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1 / wave.rate);
        }
        waveIndex++;
        //Debug.Log("--===Wave start!===--");

    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        //enemiesAlive++;
    }
}
