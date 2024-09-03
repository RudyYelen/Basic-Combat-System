using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;
    // Movement Speed Variables
    public float moveSpeed = 200f;
    public float maxSpeed = 500f;
    public float acceleration = 100f;
    public float currentSpeed = 0;

    // Dash Variables
    bool canDash = true;
    bool isDashing = false;
    public float dashPower = 50f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isDashing)
        {
            return;
        }

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

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

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector2 dashDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        rb.velocity = dashDirection * dashPower;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
