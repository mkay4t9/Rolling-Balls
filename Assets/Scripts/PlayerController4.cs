using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController4 : MonoBehaviour
{ 
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float Speed = 5.0f;
    private float PowerUpStrength = 15.0f;
    public bool hasPowerUp = false;
    public GameObject powerupIndicator;
    private float rotationSpeed = 5.0f;

    public PowerUpType currentPowerUp = PowerUpType.None;

    //Rocket PowerUp Variables
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    // Smash PowerUp Variables
    float hangTime = 0.5f;
    float smashSpeed = 5;
    float explosionForce = 50;
    float explosionRadius = 50;
    bool smashing = false;
    float floorY;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float VerticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(focalPoint.transform.forward * Speed * VerticalInput );
        playerRb.AddForce(focalPoint.transform.right * Speed * horizontalInput );

        // PowerUp Indicator Controlls
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicator.transform.Rotate(Vector3.up, rotationSpeed);
        

        if(currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        if(currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.F) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("PowerUps"))
        {
            hasPowerUp = true;
            powerupIndicator.SetActive(true);
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            Destroy(other.gameObject);

            if(powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerUpsCountdown());
        }    
    }

    IEnumerator PowerUpsCountdown()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        powerupIndicator.SetActive(false);
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.PushBack)
        {
            Rigidbody enemyPrefab = other.gameObject.GetComponent<Rigidbody>();
            Vector3 DistanceBtw = other.gameObject.transform.position - transform.position;

            enemyPrefab.AddForce(DistanceBtw * PowerUpStrength, ForceMode.Impulse);
            Debug.Log("Player collided with " + other.gameObject.name + " and with Power Up is " + currentPowerUp.ToString());
        }
    }

    void LaunchRockets()
    {
        foreach(var Enemy in FindObjectsOfType<EnemyController>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketsBehaviour>().Fire(Enemy.transform);
        }
    }

    IEnumerator Smash()
    {
        var Enemy = FindObjectsOfType<EnemyController>();

        floorY = transform.position.y;

        float jumpTime = Time.time + hangTime;

        while(Time.time < jumpTime)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        while(transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        for(int i = 0; i < Enemy.Length; i++)
        {
            if(Enemy[i] != null)
                Enemy[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }

        smashing = false;
    }
}
