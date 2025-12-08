using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
/*     public TMPro.TMP_Text tmpTextEndMessage; */

    private static WaitForSeconds _waitForSeconds1_0 = new(1.0f);
/*     private bool GameOver;
    private int PlayerScore;
    private string endMessage;
 */
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
    private bool isDashing = false;
    private float dashTimer = 0f;

    private bool canDash = true;

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
    public float wallJumpForceX = 7f;
    public float wallJumpForceY = 10f;
    private bool isWallJumping = false;
    private float wallJumpDuration = 0.2f;
    private float wallJumpTimer = 0f;

    [Header("Pausing")]
    [SerializeField] bool isPaused = false;
    public PlayerInput playerInput;



    [Header("UI")]
    public TMPro.TMP_InputField tmpIfTimeElapsed;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
/*         GameOver = false; // Game is != over at start
        PlayerScore = 0; // Player score starts at 0
        tmpTextEndMessage.text = ""; // Make sure the end message is empty
        StartCoroutine(CheckStatus()); */
    }

/*     IEnumerator CheckStatus() /// MOVED TO GameLogic.cs THIS SHOULD BE REMOVED ONCE IT WORKS
    {
        while (true)
        {
            GameObject[] keys = GameObject.FindGameObjectsWithTag("key"); // Initialize array of all keys in scene
            if (keys.Length == 0 && !GameOver) // If no keys remain and game is not over
            {
                endMessage = "Congratulations!\nYou completed the game. Your score was:\n" + PlayerScore.ToString();
                tmpTextEndMessage.text = endMessage; // Display end message
                GameOver = true;
            }
                tmpIfTimeElapsed.text = Mathf.FloorToInt(Time.timeSinceLevelLoad).ToString() + "s";
                PlayerScore ++;
                yield return _waitForSeconds1_0;
                Debug.Log("Keys remaining: " + keys.Length.ToString());
        }
    } */

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
        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
                isWallJumping = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontalMovement * movementSpeed, rb.linearVelocity.y);
        }
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
            canDash = true;
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
            float horizontal = -Mathf.Sign(transform.localScale.x) * wallJumpForceX;
            float vertical = wallJumpForceY;
            rb.linearVelocity = new Vector2(horizontal, vertical);
            isWallSliding = false;

            isWallJumping = true;
            wallJumpTimer = wallJumpDuration;

            if ((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
            {
                Flip();
            }
            
            animator.SetTrigger("jump");
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !isDashing)
        {
            float dashDirection = horizontalMovement != 0
                ? Mathf.Sign(horizontalMovement)
                : (isFacingRight ? 1f : -1f);

            isDashing = true;
            canDash = false;
            dashTimer = dashDuration;

            rb.linearVelocity = new Vector2(dashDirection * dashForce, 0f);

            Debug.Log("Dash PERFORMED");
        }
    }

    private void ResetDash()
    {
        canDash = true;
    }



    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
            canDash = true;
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

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            playerInput.enabled = false;
        }
        else
        {
            Time.timeScale = 1f;
            playerInput.enabled = true;
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

