using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CharacterMovementScript : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float moveSpeed;
    public float jumpHeight;
    private PlayerInputActions playerInput;

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

    private void Jump(InputAction.CallbackContext context)
    {
        // on jump input
        rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
        Debug.Log("right" + context.phase);
    }
}
