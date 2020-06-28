using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlatformController2D : MonoBehaviour
{
    [Header("Player Setup")]
    public float movementSpeed;
    public float jumpForce;
    public int numberOfJumps = 1;
    public float jumpFallMultiplier;                // Multiplyer to make jump fall quicker
    public float gravityMultiplier;
    [HideInInspector]
    public float movementInputDirection;

    [Header("Collision Checks")]
    public float groundCheckerRadius;
    public Transform groundChecker;
    public LayerMask groundLayer;
    [HideInInspector]
    public bool isGrounded;

    [HideInInspector]
    public Rigidbody2D rb;

    //[HideInInspector]
    public bool facingRight = true;

    private int extraJumps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityMultiplier;    // Set the gravity scale
        extraJumps = numberOfJumps;             // Set the amount of jumpa available
    }

    void Update()
    {
        CheckInput();
        CheckJump();
    }

    void FixedUpdate()
    {
        CheckPhysics();
    }

    private void CheckPhysics()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayer);
        rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);              // Move the player
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        // Flip the player according to direction
        if (!facingRight && movementInputDirection > 0f || facingRight && movementInputDirection < 0f)
        {
            Flip();
        }
    }

    private void CheckJump()
    {
        if (isGrounded)
        {
            // Reset the number of jumps allowed
            extraJumps = numberOfJumps;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpFallMultiplier) * Time.deltaTime;
        }

        // If the fire button is pressed and we have jumps available jump
        if (Input.GetButtonDown("Fire1") && extraJumps > 1)
        {
            // Apply an upward force to make the player jump
            rb.velocity = Vector2.up * jumpForce;
            // Subtract 1 jump from our jump value
            extraJumps--;
        }
        // The function for a single jump only
        else if (Input.GetButtonDown("Fire1") && isGrounded && extraJumps == 1)
        {
            // Apply an upward force to make the player jump
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    // Flip the player method
    public void Flip()
    {
        // Flip between left and right
        facingRight = !facingRight;
        // Flip the player sprite 
        transform.Rotate(0f, 180f, 0f);     
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundChecker.position, groundCheckerRadius);
    }
}
