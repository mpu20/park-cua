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
    public float upSpeed = 2.5f;
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public CinemachineVirtualCamera cinemachineCamera;
    public static bool itemActived = true;
    public static int activeItemIndex = 2;
    public static readonly List<string> items = new()
    {
        "None",
        "AquaShell",
        "WizardShell"
    };

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    private float moveInput;
    private bool canJump = true;
    public float jumpValue = 0f;
    private float facingDirection = 1f;
    private bool canDash = true;
    private bool isDashing = false;
    private List<int> alwaysActive = new() { 2 };

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

        ActiveSkill();

        ChangeAnimation();

        UpdateFacingDirection();

        UpdateMaterial();

        ActiveItem();

        Jump();
    }

    private void CheckBeOnGround()
    {
        isGrounded = Physics2D.Raycast(transform.position + Vector3.left * 0.6f, Vector2.down, 0.65f, LayerMask.GetMask("Ground")) ||
                     Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector2.down, 0.65f, LayerMask.GetMask("Ground")) ||
                     Physics2D.Raycast(transform.position, Vector2.down, 0.65f, LayerMask.GetMask("Ground"));
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
            Invoke(nameof(ResetJump), 0.2f);
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

    private void ActiveItem()
    {
        if (Input.GetKey(KeyCode.E) && !itemActived)
        {
            switch (activeItemIndex)
            {
                case 1:
                    transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    animator.Play("BubbleCreatingAnimation");
                    itemActived = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateMaterial()
    {
        if (jumpValue > 0)
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
        if (isDashing)
        {
            animator.SetInteger("State", 3); // Dashing state
        }
        else if (!isGrounded)
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

        if (jumpValue == 0f && isGrounded && (!itemActived || alwaysActive.Contains(activeItemIndex)))
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            if (moveInput != 0)
            {
                animator.Play("MovingHermitCrabAnimation");
            }
            else
            {
                animator.StopPlayback();
            }
        }
    }

    private void ActiveSkill()
    {
        if (itemActived)
        {
            switch (activeItemIndex)
            {
                case 1:
                    rb.velocity = new Vector2(moveInput, upSpeed);
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                    {
                        StartCoroutine(Dash());
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void ResetJump()
    {
        canJump = false;
        jumpValue = 0f;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        var originGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(facingDirection * dashingPower, 0f);
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = null;
        }
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.gravityScale = originGravity;
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = transform;
        }
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
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
    void DestroyBubble()
    {
        var bubbleGameObject = transform.GetChild(0).GetChild(1).gameObject;
        var bubbleController = bubbleGameObject.GetComponent<BubbleController>();
        if (bubbleController != null)
        {
            bubbleController.DestroyBubble();
        }
    }
}
