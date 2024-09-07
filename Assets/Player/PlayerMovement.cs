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
    Transform Aim;

    public float baseMoveSpeed = 200f;
    public float maxSpeed = 500f;
    public float acceleration = 100f;
    public float currentSpeed = 200f;

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
    bool bhop = false;
    public float jumpDuration = 1f;
    public float bhopTimeWindow = 0.5f;
    public float bhopForce = 100f;
    public AnimationCurve jumpCurve;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        Aim = transform.GetChild(0).transform;
    }
    
    public void MoveCharacter(Vector2 movement)
    {   
        if(isDashing)
        {
            return;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, baseMoveSpeed, maxSpeed);

        if(!isSliding)
        {
            rb.velocity = movement * currentSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = movement * currentSpeed * slideForce * Time.deltaTime;
            slideTimer -= Time.deltaTime;

            if(movement.x >= 0)
            {
                playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
            }

            if(movement.x < 0)
            {
                playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, -45f);
            }

            if(slideTimer <= 0)
            {
                StopSlide();
            }
        }
        if((movement.x != 0 || movement.y != 0) && (Input.GetAxisRaw("Horizontal") == 0 || Input.GetAxisRaw("Vertical") == 0))
        {
            Vector2 lastMovement = movement;
            Vector3 aimMovement = Vector3.left * lastMovement.x + Vector3.down * lastMovement.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, aimMovement);
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

    public void Dash(Vector2 movement)
    {
        if(canDash)
        {
            StartCoroutine(DashCo(movement));
        }
    }

    public void Jump()
    {
        if(canJump)
        {
            StartCoroutine(JumpCo()); 
        }
    }

    IEnumerator DashCo(Vector2 movement)
    {
        canDash = false;
        canJump = false;
        isDashing = true;
        //Vector2 dashmovement = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        rb.velocity = movement * dashForce;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        canJump = true;
    }

    IEnumerator JumpCo()
    {
        canJump = false;
        isJumping = true;

        if(bhop)
        {
            currentSpeed += bhopForce;;
        }

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
        StartCoroutine(bhopCo());
    }

    IEnumerator bhopCo()
    {
        bhop = true;
        yield return new WaitForSeconds(bhopTimeWindow);
        if(!isJumping)
        {
            bhop = false;
            currentSpeed = baseMoveSpeed;
        }
    }
}
