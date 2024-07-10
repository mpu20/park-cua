using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxJumpForce = 15f;
    public float jumpIncreasing = 0.1f;
    public PhysicsMaterial2D bounceMaterial, normalMaterial;
    public Vector2 boxSize;
    public float castDistance;
    public float offsetRayCast;                    

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
        CheckBeOnGround();

        Move();

        ChangeAnimation();

        UpdateFacingDirection();

        UpdateMaterial();

        Jump();        
    }

    private void CheckBeOnGround()
    {
        isGrounded = Physics2D.BoxCast(transform.position + transform.right * offsetRayCast, boxSize, 0, -transform.up, castDistance, LayerMask.GetMask("Ground"));
    }

    private void Jump()
    {
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
            ResetJump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(facingDirection * moveSpeed, jumpValue);
                jumpValue = 0f;
            }
            canJump = true;
        }
    }

    private void UpdateMaterial()
    {
        if (jumpValue == 0 && !isGrounded)
        {
            rb.sharedMaterial = bounceMaterial;
        }
        else
        {
            rb.sharedMaterial = normalMaterial;
        }
    }

    private void UpdateFacingDirection()
    {
        if (moveInput != 0 && isGrounded)
        {
            facingDirection = Mathf.Sign(moveInput);
            transform.localScale = new Vector3(facingDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void ChangeAnimation()
    {
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

        // Play landing animation
        if (!wasGrounded && isGrounded)
        {
            animator.Play("LandingHermitCrabAnimation");
        }

        wasGrounded = isGrounded;
    }

    private void Move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (jumpValue == 0f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void ResetJump()
    {
        canJump = false;
        jumpValue = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.right * offsetRayCast - transform.up * castDistance, boxSize);
    }
}
