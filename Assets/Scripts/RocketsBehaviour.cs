using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketsBehaviour : MonoBehaviour
{
    Transform target;
    float Speed = 15;
    bool homing;
    float rocketStrength = 15;
    float aliveTimer = 3;

    // Update is called once per frame
    void Update()
    {
        if(homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * Speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        Destroy(gameObject, aliveTimer);
    }

    private void OnCollisionEnter(Collision other) {
        if(target != null)
        {
            if(other.gameObject.CompareTag(target.tag))
            {
                Rigidbody targetRb = other.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -other.contacts[0].normal;
                targetRb.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}
