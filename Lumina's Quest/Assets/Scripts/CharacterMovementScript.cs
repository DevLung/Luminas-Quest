using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementScript : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public CircleCollider2D feetTrigger;
    public Animator animator;
    public PlayerInputActions playerInput;
    public float moveSpeed;
    public float jumpHeight;
    public int frameBufferSize;
    public bool doubleJump;
    private int jumpCount;
    private int frameOfLastFailedJumpInput;
    private bool lastJumpSuccessful = true;
    private Collider2D lastCollider;
    private Vector2 lastPosition;


    private void Start()
    {
        // enable Movement InputActions
        playerInput = new PlayerInputActions();
        playerInput.Movement.Enable();
        // subscribe Jump method to performed event of jump input
        playerInput.Movement.jump.performed += JumpInput;
    }


    private void Update()
    {
        // move on move input
        Vector2 inputVector = playerInput.Movement.move.ReadValue<Vector2>();
        rigidBody.transform.position += moveSpeed * Time.deltaTime * new Vector3(inputVector.x, 0, 0);
    }


    private void FixedUpdate()
    {
        // set onGround parameter in animator
        animator.SetBool("onGround", lastCollider != null && feetTrigger.IsTouching(lastCollider));
        // sync character velocity with velocity parameters in animator to control when the running animation is triggered
        animator.SetFloat("xVelocity", transform.position.x - lastPosition.x);
        animator.SetFloat("yVelocity", transform.position.y - lastPosition.y);
        lastPosition = transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // save data of last collider
        lastCollider = collision;

        // jump if an unsuccessful jump input occurred less than <frameBufferSize> frames ago and the character reaches the floor
        if (!lastJumpSuccessful && Time.frameCount - frameOfLastFailedJumpInput < frameBufferSize)
        {
            Jump();
        }
    }


    private void JumpInput(InputAction.CallbackContext context)
    {
        // on jump input while on floor, only if the character feet are above the top surface of floor or after first jump if double jump is enabled
        if (jumpCount == 1 || feetTrigger.IsTouching(lastCollider))
        {
            Jump();
        }
        else
        {
            frameOfLastFailedJumpInput = Time.frameCount;
            lastJumpSuccessful = false;
        }
    }


    private void Jump()
    {
        // compensate for downwards velocity so jump is always equally high
        float lastNegativeVelocity = 0;
        if (rigidBody.GetRelativePointVelocity(Vector2.zero).y < 0)
        {
            lastNegativeVelocity = -rigidBody.velocity.y;
        }
        // jump
        rigidBody.AddForce(Vector3.up * (jumpHeight + lastNegativeVelocity), ForceMode2D.Impulse);
        animator.Play("jumpup");

        // increment jump count if double jump is enabled and reset if it is higher than 1 so you can only jump in the air once 
        if (doubleJump)
        {
            if (jumpCount > 1 || feetTrigger.IsTouching(lastCollider))
            {
                jumpCount = 0;
            }
            jumpCount++;
        }

        lastJumpSuccessful = true;
    }
}