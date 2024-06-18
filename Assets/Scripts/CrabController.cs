using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxJumpForce = 15f;
    public float minJumpForce = 5f;
    public float chargeTime = 1f;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isChargingJump;
    private float jumpCharge;
    private float chargeStartTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

        float move = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(move, rb.velocity.y);
        animator.SetInteger("State", move != 0 ? 1 : 0);

        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isChargingJump = true;
            chargeStartTime = Time.time;
        }

        if (isChargingJump)
        {
            if (Input.GetButton("Jump"))
            {
                jumpCharge = Mathf.Lerp(minJumpForce, maxJumpForce, (Time.time - chargeStartTime) / chargeTime);
            }

            if (Input.GetButtonUp("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpCharge);
                isChargingJump = false;
                jumpCharge = 0;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f);
    }
}
