using CoinSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;

    [Header("Movement")]
    public float movementSpeed = 5f;
    public float horizontalMovement;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    int jumpsRemaining;

    [Header("Dashing")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    bool isDashing = false;
    float dashTimer = 0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fastFallMultiplier = 2f;

    [Header("Wall Check")]
    public Transform wallCheck;
    public Vector2 wallCheckSize = new Vector2(0.05f, 0.5f);
    public LayerMask wallLayer;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;
    float wallJumpDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
                isDashing = false;

            return;
        }
        rb.linearVelocity = new Vector2(horizontalMovement * movementSpeed, rb.linearVelocity.y);
        GroundCheck();
        Gravity();
        Flip();
        ProcessWallSlide();
        
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
    }

    private void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fastFallMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ProcessWallSlide()
    {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
            jumpsRemaining = maxJumps;
        }
        else
        {
            isWallSliding = false;
        }

    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
        }
        if (isWallSliding && context.performed)
        {
            rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * movementSpeed, jumpForce);
            wallJumpDirection = -Mathf.Sign(transform.localScale.x);
            isWallSliding = false;
            animator.SetTrigger("jump");
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing)
        {
            float dashDirection = horizontalMovement != 0
                ? Mathf.Sign(horizontalMovement)
                : (isFacingRight ? 1f : -1f);

            isDashing = true;
            dashTimer = dashDuration;

            rb.linearVelocity = new Vector2(dashDirection * dashForce, 0f);

            Debug.Log("Dash PERFORMED");
        }
    }



    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, wallLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);

    }
}

