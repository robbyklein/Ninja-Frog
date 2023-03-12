using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumping : MonoBehaviour
{
    // Components
    private PlayerMovement playerMovement;
    private PlayerState playerState;
    private Rigidbody2D rb;

    // Constants
    private const float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    // Jumping
    public bool isWallJumping;
    private bool jumpQueued = false;
    private bool wallJumpQueued = false;
    private const float queueDuration = 0.15f;

    [SerializeField] private float jumpForce = 21f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15f, 21f);
    [SerializeField] private float slideSpeed = 6f;
    [SerializeField] private float wallJumpDuration = .2f;
    [SerializeField] private AudioSource jumpSound;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerState = GetComponent<PlayerState>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCoyoteTime();
        HandleQueuedJumps();
        HandleQueuedWallJumps();
    }

    private void HandleCoyoteTime()
    {
        if (playerState.IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    // Jumping
    private void OnJump(InputValue value)
    {
        bool buttonDown = value.Get<float>() == 1;

        if (buttonDown && coyoteTimeCounter > 0f)
        {
            Jump();
        }
        else if (buttonDown && playerState.IsWalled())
        {
            WallJump();
        }
        else if (buttonDown && playerState.IsAlmostGrounded())
        {
            jumpQueued = true;
            Invoke("ClearJumpQueued", queueDuration);
        }
        else if (buttonDown && playerState.IsAlmostWalled())
        {
            wallJumpQueued = true;
            Invoke("ClearWallJumpQueued", queueDuration);
        }
        else if (!buttonDown && rb.velocity.y > 0.1f && !isWallJumping)
        {
            // Shorten jump early if not already falling
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            coyoteTimeCounter = 0f; // Clear late jumping

        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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


        if (isWallJumping)
        {
            // Jump out and up
            jumpSound.Play();
            rb.velocity = new Vector2(-playerMovement.movementInput.x * wallJumpForce.x, wallJumpForce.y);
        }
    }

    private void HandleQueuedWallJumps()
    {
        if (wallJumpQueued && playerState.IsWalled())
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
