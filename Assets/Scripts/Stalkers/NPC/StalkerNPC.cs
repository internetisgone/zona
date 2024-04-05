using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerNPC : CStalker
{

    private GameObject campfire; // temp
    private Rigidbody rb;

    void Start()
    {
        campfire = GameObject.FindWithTag("Campfire");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 movement = campfire.transform.position - transform.position;

        if(movement.magnitude > 3)
        {
            // move towards campfire
            //Debug.LogFormat("{0} is {1} meters away from campfire, speed {2}", Name, movement.magnitude, Speed);
            movement.y = 0;

            MoveStalker(movement.normalized);
        }
    }

    private void FixedUpdate()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(Name + ": Get out of here stalker ");
        }
    }
}
