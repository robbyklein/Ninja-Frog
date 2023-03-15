using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumping : MonoBehaviour
{
    // Components
    private PlayerMovement playerMovement;
    private PlayerSpriteState playerSpriteState;
    private Rigidbody2D rb;

    // Settings
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float queueDuration = 0.15f;
    [SerializeField] private float jumpForce = 21f;
    [SerializeField] private float runningAdditionalJumpForce = 5f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15f, 21f);
    [SerializeField] private float slideSpeed = 6f;
    [SerializeField] private float wallJumpDuration = .2f;
    [SerializeField] private AudioSource jumpSound;

    // State
    private float coyoteTimeCounter;
    private float coyoteTimeCounterWall;
    public bool isWallJumping;
    private bool jumpQueued = false;
    private bool wallJumpQueued = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerSpriteState = GetComponent<PlayerSpriteState>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateCoyoteTimeCounter();
        HandleQueuedJumps();
        HandleQueuedWallJumps();
    }

    private void UpdateCoyoteTimeCounter()
    {
        if (playerSpriteState.IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (playerSpriteState.IsWalled())
        {
            coyoteTimeCounterWall = coyoteTime;
        }
        else
        {
            coyoteTimeCounterWall -= Time.deltaTime;
        }
    }

    private void OnJump(InputValue value)
    {
        bool buttonDown = value.Get<float>() == 1;

        if (buttonDown && coyoteTimeCounter > 0f)
        {
            Jump();
        }
        else if (buttonDown && coyoteTimeCounterWall > 0f)
        {
            WallJump();
        }
        else if (buttonDown && playerSpriteState.IsAlmostGrounded())
        {
            jumpQueued = true;
            Invoke("ClearJumpQueued", queueDuration);
        }
        else if (buttonDown && playerSpriteState.IsAlmostWalled())
        {
            wallJumpQueued = true;
            Invoke("ClearWallJumpQueued", queueDuration);
        }
        else if (!buttonDown && rb.velocity.y > 0.1f && !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            coyoteTimeCounter = 0f;

        }
    }

    private void Jump()
    {
        // Perform jump
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // Play sound effect
        jumpSound.Play();
    }

    private void HandleQueuedJumps()
    {
        if (jumpQueued && coyoteTimeCounter > 0f)
        {
            ClearJumpQueued();
            Jump();
        }
    }

    private void ClearJumpQueued()
    {
        jumpQueued = false;
    }

    private void WallJump()
    {
        // Set delay before user gets control again
        isWallJumping = true;
        Invoke("FinishWallJump", wallJumpDuration);

        // Jump out and up
        jumpSound.Play();

        float xVelocity = playerMovement.lastSlideRight ? -1f : 1f;
        rb.velocity = new Vector2(xVelocity * wallJumpForce.x, wallJumpForce.y);
    }

    private void HandleQueuedWallJumps()
    {
        if (wallJumpQueued && coyoteTimeCounter > 0f)
        {
            ClearWallJumpQueued();
            WallJump();
        }
    }

    private void FinishWallJump()
    {
        isWallJumping = false;
    }

    private void ClearWallJumpQueued()
    {
        wallJumpQueued = false;
    }
}
