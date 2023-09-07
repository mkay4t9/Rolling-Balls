using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] enemyPrefab;

    private float spawnRange = 9.0f;

    public int enemyCounter;

    private int EnemyWave = 1;
    public GameObject[] powerupsPrefab; 

    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;

    public int bossRound;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(powerupsPrefab, GenerateRandomPos(), enemyPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCounter = FindObjectsOfType<EnemyController>().Length;
        if(enemyCounter == 0)
        {
            if (EnemyWave % bossRound == 0)
            {
                SpawnBosswave(EnemyWave);
            }
            else
                SpawnEnemies(EnemyWave);
            int randomPowerup = Random.Range(0, powerupsPrefab.Length);
            Instantiate(powerupsPrefab[randomPowerup], GenerateRandomPos(), powerupsPrefab[randomPowerup].transform.rotation);
            
            EnemyWave++;
        }
    }

    private Vector3 GenerateRandomPos()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }

    void SpawnBosswave(int currentRound)
    {
        int miniEnemiesToSpawn;

        if(bossRound != 0)
        {
            miniEnemiesToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemiesToSpawn = 1;
        }

        var boss = Instantiate(bossPrefab, GenerateRandomPos(), bossPrefab.transform.rotation);
        boss.GetComponent<EnemyController>().miniEnemySpawnCount = miniEnemiesToSpawn;
    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateRandomPos(), miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }

    void SpawnEnemies(int enemiesToSpawn)
    {
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[randomEnemy], GenerateRandomPos(), enemyPrefab[randomEnemy].transform.rotation);
        }
    }
}
