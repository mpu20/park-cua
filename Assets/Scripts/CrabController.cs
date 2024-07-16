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
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float jumpValue = 0f;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    private float moveInput;
    private bool isCharging = false;
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpValue = 0f;
        }

        if (isCharging)
        {
            jumpValue += jumpIncreasing;
            if (jumpValue > maxJumpForce)
            {
                jumpValue = maxJumpForce;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            Debug.Log(facingDirection * moveSpeed + " " + jumpValue);
            rb.velocity = new Vector2(facingDirection * moveSpeed, jumpValue);
            jumpValue = 0f;
            isCharging = false;
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
        else if (moveInput != 0 && isCharging)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
