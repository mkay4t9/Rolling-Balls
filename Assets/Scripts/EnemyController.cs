using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody enemyRb;
    public float Speed = 5.0f;
    private GameObject player;

    public bool isBoss = false;
    public float spawnInterval;
    private float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnController spawnController;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if(isBoss)
        {
            spawnController = FindObjectOfType<SpawnController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.AddForce((player.transform.position - transform.position).normalized * Speed);

        if(isBoss)
        {
            if(Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnController.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }

        if(transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
}
