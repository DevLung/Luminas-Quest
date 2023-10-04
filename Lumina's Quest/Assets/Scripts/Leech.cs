using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leech : MonoBehaviour
{
    public float moveSpeed = 1.0f;  // speed of the leech

    private Rigidbody2D rb2d;
    private bool movingRight = true;
    private Animator animator;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        float horizontalMovement = movingRight ? 1.0f : -1.0f;

        
        rb2d.velocity = new Vector2(horizontalMovement * moveSpeed, rb2d.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("floor"))
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
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
