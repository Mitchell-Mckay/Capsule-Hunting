using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spawner : MonoBehaviour
{   
    public int waveNumber = 0;
    public int enemySpawnAmount = 0;
    public int enemiesKilled = 0;

    [Header("Round Change")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    public GameObject[] spawners;
    public GameObject enemy;

    public GameObject[] powerups;

    [SerializeField] TextMeshProUGUI roundAmount;

    public float timeBetweenPowerups = 30f;
    float timer;

    public AudioClip powerupSpawn;

    private void Start()
    {
        spawners = new GameObject[8];

        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

        StartWave();

        UpdateRoundUI();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > timeBetweenPowerups)
        {
            SpawnPowerup();
            timer = 0;
        }

        if (waveNumber >= 10)
        {
            timeBetweenPowerups = 60f;
        }
        else
        {
            timeBetweenPowerups = 30f;
        }
    }

    private void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);
        Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
    }

    private void SpawnPowerup()
    {
        audioSource.PlayOneShot(powerupSpawn);
        int spawnerID = Random.Range(0, spawners.Length);
        int powerupID = Random.Range(0, powerups.Length);
        GameObject spawnedPowerup = Instantiate(powerups[powerupID], spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
        if (powerupID == 0) 
        {
            spawnedPowerup.tag = "MaxHealthClone";
        }
        else if (powerupID == 1) 
        {
            spawnedPowerup.tag = "MaxAmmoClone";
        }
        else if (powerupID == 2) 
        {
            spawnedPowerup.tag = "RootBeerClone";
        }
        else if (powerupID == 3) 
        {
            spawnedPowerup.tag = "SpeedBoostClone";
        }
    }

    private void StartWave()
    {
        waveNumber = 1;
        enemySpawnAmount = 2;
        enemiesKilled = 0;

        for(int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }
    }

    public void NextWave()
    {
        waveNumber++;
        enemySpawnAmount += 2;
        enemiesKilled = 0;
        audioSource.PlayOneShot(RandomClip());
        UpdateRoundUI();

        for(int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }
    }

    AudioClip RandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length-1)];
    }

    private void UpdateRoundUI()
    {
        roundAmount.text = waveNumber.ToString();
    }
}
