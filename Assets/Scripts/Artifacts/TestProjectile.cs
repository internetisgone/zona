using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{
    private float speed = 0f;
    private float ySpeed = 0f;

    private float mass = 1;
    private float force = 30f;
    private float drag = 0.1f;

    private float gravity = -9.8f;
    private float gravityAcceleration;
    

    void Start()
    {
        float acceleration = force / mass;
        speed += acceleration;
        gravityAcceleration = gravity / mass;
    }

    private void Update()
    {
        if(transform.position.y < - 2)
        {
           Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // gravity
        ySpeed -= gravityAcceleration * Time.deltaTime;
        // drag
        float dragFactor = 1 - drag * Time.deltaTime; 

        Vector3 movement = (transform.forward * speed + ySpeed * Vector3.down) * dragFactor;

        transform.Translate(movement * Time.deltaTime);

        
    }
}
