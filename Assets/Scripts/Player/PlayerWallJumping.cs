using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallJumping : MonoBehaviour
{
    // Components
    private PlayerMovement playerMovement;
    private PlayerSpriteState playerSpriteState;
    private Rigidbody2D rb;
    private PlayerSliding playerSliding;

    // Settings
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15f, 21f);
    [SerializeField] private float wallJumpDuration = .2f;
    [SerializeField] private float queueDuration = 0.15f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private AudioSource jumpSound;

    // State
    private float coyoteTimeCounter;
    private bool wallJumpQueued = false;
    public bool isWallJumping { get; private set; }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerSpriteState = GetComponent<PlayerSpriteState>();
        playerSliding = GetComponent<PlayerSliding>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateCoyoteTimeCounter();
        HandleQueuedWallJumps();
    }

    private void OnJump(InputValue value)
    {
        bool buttonDown = value.Get<float>() == 1;

        if (buttonDown && coyoteTimeCounter > 0f)
        {
            WallJump();
        }
        else if (buttonDown && playerSpriteState.IsAlmostWalled())
        {
            wallJumpQueued = true;
            Invoke("ClearWallJumpQueued", queueDuration);
        }
    }

    void UpdateCoyoteTimeCounter()
    {
        if (playerSpriteState.IsWalled())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void WallJump()
    {
        // Set local state
        isWallJumping = true;

        // Call clear local state function
        Invoke("FinishWallJump", wallJumpDuration);

        // Play a sound
        jumpSound.Play();

        // Perform movement
        float xDirection = playerSliding.lastSlideRight ? -1f : 1f;
        rb.velocity = new Vector2(xDirection * wallJumpForce.x, wallJumpForce.y);
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
        // Update local state
        isWallJumping = false;
    }

    private void ClearWallJumpQueued()
    {
        wallJumpQueued = false;
    }
}
