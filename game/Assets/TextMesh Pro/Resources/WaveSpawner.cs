using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveSpawner : MonoBehaviour
{
public enum SpawnState {SPAWNING, WAITING, COUNTING }  
 [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private float waveCountdown = 0;
    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;
    public void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn Points refferenced.");
        }
        waveCountdown = timeBetweenWaves;
    }
    public void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if (!enemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {

                return;
            }
        }
        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }else
        {
            waveCountdown -= Time.deltaTime;
        }
    }
    void WaveCompleted()
    {
        Debug.Log("wave completed!");
        /*        logMessage = "wave completed!;
              ;
        System.IO.File.WriteAllText(@"/Users/user/game/WriteText.txt", logMessage);
        Debug.Log(logMessage)*/
       state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("ALL ways complete! Looping...");
        }
        else
        {
            nextWave++;
        }
    }
    bool enemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("enemy") == null)
            {

                return false;
            }
        }
        return true;
    }
    IEnumerator SpawnWave (Wave _wave)
    {
        Debug.Log("spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;
        for (int i=0;i< _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }
    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("spawning enemy " + _enemy.name);
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation );

    }
}
