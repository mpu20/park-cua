using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxJumpForce = 15f;
    public float jumpIncreasing = 0.1f;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    private float moveInput;
    private bool canJump = true;
    private float jumpValue = 0f;
    private float facingDirection = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics2D.Raycast(transform.position + Vector3.left * 0.6f, Vector2.down, 0.65f, LayerMask.GetMask("Ground")) ||
                     Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector2.down, 0.65f, LayerMask.GetMask("Ground"));

        moveInput = Input.GetAxisRaw("Horizontal");

        // Handle movement
        if (jumpValue == 0f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        // Update animator states
        if (!isGrounded)
        {
            animator.SetInteger("State", 2); // Jumping state
        }
        else if (moveInput != 0)
        {
            animator.SetInteger("State", 1); // Walking state
        }
        else
        {
            animator.SetInteger("State", 0); // Idle state
        }

        // Update facing direction
        if (moveInput != 0)
        {
            facingDirection = Mathf.Sign(moveInput);
            transform.localScale = new Vector3(facingDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Handle jump charging
        if (Input.GetKey(KeyCode.Space) && isGrounded && canJump)
        {
            jumpValue += jumpIncreasing;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        if (jumpValue >= maxJumpForce && isGrounded)
        {
            rb.velocity = new Vector2(facingDirection * moveSpeed, jumpValue);
            Invoke(nameof(ResetJump), 0.1f);
        }

        if (Input.GetKeyUp(KeyCode.Space) )
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(facingDirection * moveSpeed, jumpValue);
                jumpValue = 0f;
            }
            canJump = true;
        }

        // Play landing animation
        if (!wasGrounded && isGrounded)
        {
            animator.Play("LandingHermitCrabAnimation");
        }

        wasGrounded = isGrounded;
    }

    void ResetJump()
    {
        canJump = false;
        jumpValue = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var leftPosition = transform.position + Vector3.left * 0.6f;
        var rightPosition = transform.position + Vector3.right * 0.5f;
        var downDistance = Vector3.down * 0.65f;
        Gizmos.DrawLine(leftPosition, leftPosition + downDistance);
        Gizmos.DrawLine(rightPosition, rightPosition + downDistance);
    }
}
