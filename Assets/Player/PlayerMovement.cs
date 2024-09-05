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
    SpriteRenderer playerSprite;
    Vector2 movement;

    public float moveSpeed = 200f;
    public float maxSpeed = 500f;
    public float acceleration = 100f;
    public float currentSpeed = 0;

    bool isSliding = false;
    public float maxSlideTime = 0.75f;
    public float slideForce = 3f;
    float slideTimer;

    bool canDash = true;
    bool isDashing = false;
    public float dashForce = 50f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.5f;

    bool canJump = true;
    bool isJumping = false;
    public float jumpDuration = 1f;
    public AnimationCurve jumpCurve;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
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

        if(!isSliding)
        {
            rb.velocity = direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = direction * moveSpeed * slideForce * Time.deltaTime;
            slideTimer -= Time.deltaTime;

            if(direction.x >= 0)
            {
                playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
            }

            if(direction.x < 0)
            {
                playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, -45f);
            }

            if(slideTimer <= 0)
            {
                StopSlide();
            }
        }
    }

    public void StartSlide()
    {
        isSliding = true;
        canDash = false;
        slideTimer = maxSlideTime;

        
    }

    public void StopSlide()
    {
        if(isSliding)
        {
            isSliding = false;
            canDash = true;
            playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
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
        rb.velocity = dashDirection * dashForce;
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
            playerSprite.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpPrecentage);

            if(jumpPrecentage == 1f)
            {
                break;
            }

            yield return null;
        }

        playerSprite.transform.localScale = Vector3.one;
        isJumping = false;
        canJump = true;
    }
}
