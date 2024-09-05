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
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if(Input.GetKeyDown(KeyCode.LeftShift))
            playerMovement.Dash();

        if(Input.GetKeyDown(KeyCode.Z))
            playerMovement.Jump();
    }

    void FixedUpdate()
    {
        playerMovement.MoveCharacter(movement);
    }
}
