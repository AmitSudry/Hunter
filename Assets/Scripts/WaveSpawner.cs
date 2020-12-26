using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPWANING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float delay;
    }

    public TextMeshProUGUI WaveCompletedText;

    private Wave[] waves;
    private int nextWave = 0;
    private int numOfWaves;

    public Transform[] optionalEnemies;
    
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5.0f;
    public float waveCountdown;

    private float searchCountDown = 1.0f;

    private SpawnState state = SpawnState.COUNTING;

    void OnEnable()
    {
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0 || optionalEnemies.Length == 0)
        {
            Debug.Log("No referenced locations or optional enemies");
            return;
        }

        //Debug.Log("Difficulty: " + PlayerPrefs.GetInt("CurrentDifficulty"));
        int minWaves = 1, maxWaves = 1, minCount = 1, maxCount = 1;
        if(PlayerPrefs.GetInt("CurrentDifficulty") == 0) //easy
        {
            minWaves = 2;
            maxWaves = 3;
            minCount = 1;
            maxCount = 3;
        }
        else if (PlayerPrefs.GetInt("CurrentDifficulty") == 1) //medium
        {
            minWaves = 2;
            maxWaves = 4;
            minCount = 2;
            maxCount = 4;
        }
        else if (PlayerPrefs.GetInt("CurrentDifficulty") == 2) //hard
        {
            minWaves = 3;
            maxWaves = 6;
            minCount = 2;
            maxCount = 5;
        }

        numOfWaves = Random.Range(minWaves, maxWaves + 1); //pick number of waves
        //Debug.Log("Number of waves: " + numOfWaves);

        //Generate Random level
        waves = new Wave[numOfWaves];
        for (int i = 0; i < waves.Length; i++)
        {

            Wave w = new Wave();
            if(i==0)
            {
                int indexOfEnemy = Random.Range(0, optionalEnemies.Length); //pick index of enemy to spawn
                w.enemy = optionalEnemies[indexOfEnemy];
            }
            else
            {
                //Same enemy two waves in a row is not allowed
                do
                {
                    int indexOfEnemy = Random.Range(0, optionalEnemies.Length); //pick index of enemy to spawn
                    w.enemy = optionalEnemies[indexOfEnemy];
                }
                while (waves[i - 1].enemy == w.enemy);
            }
            
            w.count = Random.Range(minCount, maxCount + 1);
            w.delay = 0.75f;
            w.name = "Wave #" + i.ToString();
            waves[i] = w;
            //Debug.Log("Enemy index: " + indexOfEnemy + ", Count: " + w.count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if(!EnemyLeft()) //Wave completed
            {
                //Debug.Log("Wave Completed");
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0.0f)
        {
            if (state != SpawnState.SPWANING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }
    
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(++nextWave == waves.Length)
        {
            nextWave = 0;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Win");
        }
        else
        {
            WaveCompletedText.gameObject.SetActive(true);
            WaveCompletedText.SetText("Completed wave " + nextWave + " / " + numOfWaves);
            //Debug.Log("Completed wave " + nextWave + "/" + numOfWaves);
        }
    }

    bool EnemyLeft()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0.0f)
        {
            searchCountDown = 1.0f;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
       
        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPWANING;

        WaveCompletedText.gameObject.SetActive(false);

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(wave.delay);
        }
        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform enemy)
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, sp.position, sp.rotation);
    }
}
