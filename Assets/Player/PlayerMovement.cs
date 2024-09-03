using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 200f;
    public float maxSpeed = 500f;
    public float acceleration = 100f;
    public float currentSpeed = 0;
    Rigidbody2D rb;
    Vector2 movement;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        MoveCharacter(movement);
    }
    
    void MoveCharacter(Vector2 direction)
    {   
        rb.velocity = direction * moveSpeed * Time.deltaTime;
        /*if(Math.Abs(direction.x) > 0 || Math.Abs(direction.y) > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = 0;
        }
        
        currentSpeed = Math.Clamp(currentSpeed, 0, maxSpeed);
        rb.velocity = direction * currentSpeed * Time.deltaTime;*/
        //rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}
