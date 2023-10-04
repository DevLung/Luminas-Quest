using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leech : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Animator animator;
    public float moveSpeed;
    private bool movingRight = true;


    void FixedUpdate()
    {
        // make velocity negative when moving left
        float horizontalMovement = movingRight ? 1 : -1;

        // move leech
        rigidBody.velocity = new Vector2(horizontalMovement * moveSpeed, rigidBody.velocity.y);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("floor"))
        {
            // Change direction when hitting an obstacle.
            FlipDirection();
        }
    }


    void FlipDirection()
    {
        // Flip the leech direction.
        movingRight = !movingRight;

        // Flip the leech sprite horizontally.
        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;
    }
}
