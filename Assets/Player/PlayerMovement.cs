using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;

    public float moveSpeed = 200f;
    public float maxSpeed = 500f;
    public float acceleration = 100f;
    public float currentSpeed = 0;

    bool canDash = true;
    bool isDashing = false;
    public float dashPower = 50f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.5f;

    bool canJump = true;
    bool isJumping = false;
    public float jumpDuration = 1f;
    public AnimationCurve jumpCurve;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }
    
    public void MoveCharacter(Vector2 direction)
    {   
        if(isDashing)
        {
            return;
        }
        rb.velocity = direction * moveSpeed * Time.deltaTime;
    }
    public void Dash()
    {
        if(canDash)
        {
            StartCoroutine(DashCo());
        }
    }

    public void Jump()
    {
        if(canJump)
        {
            StartCoroutine(JumpCo());
        }
    }

    IEnumerator DashCo()
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

    IEnumerator JumpCo()
    {
        canJump = false;
        isJumping = true;

        float jumpStartTime = Time.time;

        while(isJumping)
        {
            float jumpPrecentage = (Time.time - jumpStartTime) / jumpDuration;
            jumpPrecentage = Mathf.Clamp01(jumpPrecentage);
            transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpPrecentage);

            if(jumpPrecentage == 1f)
            {
                break;
            }

            yield return null;
        }

        transform.localScale = Vector3.one;
        isJumping = false;
        canJump = true;
    }
}
