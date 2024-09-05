using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerMovement playerMovement;
    Vector2 movement;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movement = new Vector2(horizontal, vertical).normalized;

        if(Input.GetKeyDown(KeyCode.LeftShift))
            playerMovement.Dash();

        if(Input.GetKeyDown(KeyCode.LeftControl) && (horizontal != 0 || vertical != 0))
            playerMovement.StartSlide();

        if(Input.GetKeyUp(KeyCode.LeftControl))
            playerMovement.StopSlide();

        if(Input.GetKeyDown(KeyCode.Space))
            playerMovement.Jump();
    }

    void FixedUpdate()
    {
        playerMovement.MoveCharacter(movement);
    }
}
