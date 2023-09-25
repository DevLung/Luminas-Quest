using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CharacterMovementScript : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public CapsuleCollider2D capsuleCollider;
    public float moveSpeed;
    public float jumpHeight;
    public bool doubleJump;
    private int jumpCount;
    public PlayerInputActions playerInput;
    private Collision2D lastCollision;

    private void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Movement.Enable();
        playerInput.Movement.jump.performed += Jump;
    }

    private void Update()
    {
        // on move input
        Vector2 inputVector = playerInput.Movement.move.ReadValue<Vector2>();
        rigidBody.transform.position += new Vector3(inputVector.x, 0, 0) * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        lastCollision = collision;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        // on jump input while on floor or after first jump if double jump is enabled
        if (jumpCount == 1 || capsuleCollider.IsTouching(lastCollision.collider) && lastCollision.collider.tag == "floor")
        {
            // calculate height of character feet and top surface of floor and only allow jump if the character feet are above the top surface of floor
            float feetHeight = capsuleCollider.transform.position.y - capsuleCollider.bounds.extents.y;
            float FloorHeight = lastCollision.collider.transform.position.y + lastCollision.collider.bounds.extents.y;
            if (jumpCount == 1 || feetHeight >= FloorHeight)
            {
                float lastNegativeVelocity = 0;
                if (rigidBody.GetRelativePointVelocity(Vector2.zero).y < 0)
                {
                    lastNegativeVelocity = -rigidBody.GetRelativePointVelocity(Vector2.zero).y;
                }
                rigidBody.AddForce(Vector3.up * (jumpHeight + lastNegativeVelocity), ForceMode2D.Impulse);
                lastNegativeVelocity = 0;
                // increment jump count if double jump is enabled and reset if it is higher than 1 so you can only jump in the air once 
                if (doubleJump)
                {
                    if (jumpCount > 1 || capsuleCollider.IsTouching(lastCollision.collider) && lastCollision.collider.tag == "floor" && feetHeight >= FloorHeight)
                    {
                        jumpCount = 0;
                    }
                    jumpCount++;
                }
            }
        }
    }
}
