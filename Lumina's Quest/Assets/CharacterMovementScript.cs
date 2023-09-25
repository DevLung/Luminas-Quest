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
    private PlayerInputActions playerInput;
    private Collision2D lastCollision;

    private void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Movement.Enable();
        playerInput.Movement.jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        // on move input
        Vector2 inputVector = playerInput.Movement.move.ReadValue<Vector2>();
        rigidBody.transform.position += new Vector3(inputVector.x, 0, 0) * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        lastCollision = collision;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        // on jump input while on floor
        if (capsuleCollider.IsTouching(lastCollision.collider) && lastCollision.collider.tag == "floor")
        {
            float feetHeight = capsuleCollider.transform.position.y - capsuleCollider.bounds.extents.y;
            float FloorHeight = lastCollision.collider.transform.position.y + lastCollision.collider.bounds.extents.y;
            if (feetHeight >= FloorHeight)
            {
                rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
            }
        }
    }
}
