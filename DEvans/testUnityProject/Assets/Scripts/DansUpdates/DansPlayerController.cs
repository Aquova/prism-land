﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DansPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startingPositon;
    private bool canMove;
    public float speed;
    public float Showtime = 0f;
    public Transform cam;
    //private bool ground;
    //public float jump;
    public bool jump1;
    public float jumpSpeed;
    //public float raycast;
    private float speedInitial;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingPositon = transform.position;
        speedInitial = speed;
        canMove = true;
        jump1 = true;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = cam.TransformDirection(movement);
        // We need to detach jump movement from the camera angle

        if (Showtime > 0f)
        {
            Showtime = Showtime - (Time.deltaTime);
        }
        else
        {
            speed = speedInitial;
        }

        // Changed to only make horizontal movements affect while grounded
        //if (ground)
        //{

        //}

        // Jump stuff
        //        if (Physics.Raycast(transform.position, Vector3.down, raycast))
        //        {
        //            ground = true;
        //        }
        //        else
        //        {
        //           ground = false;
        //        }

        if (jump1 && Input.GetButtonDown("Jump"))
        {
            // TODO: Add ray debug draw to make sure raycasting is in the right direction
            movement.y = jumpSpeed;
            jump1 = false;
        }
        else
        {
            movement.y = 0;
        }

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("We have collided " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            speed = 40f;
            Showtime = 3f;
        }

        if (other.gameObject.CompareTag("Bounce"))
        {
            Debug.Log("trampoline");
            GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
        }
        if (other.gameObject.CompareTag("Death"))
        {
            rb.position = startingPositon;
            rb.velocity = rb.velocity * 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SafeToJump"))
        {
            print("hit Safe to jump");
            jump1 = true;
        }
        if (collision.gameObject.CompareTag("Boom"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000.0f, collision.gameObject.GetComponent<Rigidbody>().position, 30000.0f);
        }
    }
    public void Stop()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void ResetPosition()
    {
        Stop();
        transform.position = startingPositon;
    }
}