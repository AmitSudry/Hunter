using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    private Wave[] waves;
    private int nextWave = 0;
    public int maxWaves = 8;
    public int minWaves = 3;

    public int minCount = 1;
    public int maxCount = 5;

    public Transform[] optionalEnemies;
    
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5.0f;
    public float waveCountdown;

    private float searchCountDown = 1.0f;

    private SpawnState state = SpawnState.COUNTING;

    // Start is called before the first frame update
    void Start()
    {
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0 || optionalEnemies.Length == 0)
        {
            Debug.Log("No referenced locations or optional enemies");
            return;
        }
        int numOfWaves = Random.Range(minWaves, maxWaves+1); //pick number of waves

        //Generate Random level
        waves = new Wave[numOfWaves];
        for(int i=0; i<numOfWaves; i++)
        {
            int indexOfEnemy = Random.Range(0, optionalEnemies.Length); //pick index of enemy to spawn
            waves[i].enemy = optionalEnemies[indexOfEnemy];
            waves[i].count = Random.Range(minCount, maxCount + 1);
            waves[i].delay = 0.5f;
            waves[i].name = "Wave #" + i.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if(!EnemyLeft()) //Wave completed
            {
                Debug.Log("Wave Completed");
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
