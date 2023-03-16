using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumping : MonoBehaviour
{
    // Components
    private PlayerSpriteState playerSpriteState;
    private PlayerWallJumping playerWallJumping;
    private Rigidbody2D rb;
    private PlayerHelpers playerHelpers;

    // Settings
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float queueDuration = 0.15f;
    [SerializeField] private float jumpForce = 21f;
    [SerializeField] private AudioSource jumpSound;

    // State
    private float coyoteTimeCounter;
    private bool jumpQueued = false;

    // Event
    public event Action OnJumpTriggered;

    private void Start()
    {
        playerSpriteState = GetComponent<PlayerSpriteState>();
        playerWallJumping = GetComponent<PlayerWallJumping>();
        rb = GetComponent<Rigidbody2D>();
        playerHelpers = GetComponent<PlayerHelpers>();
    }

    private void Update()
    {
        UpdateCoyoteTimeCounter();
        HandleQueuedJumps();
    }

    private void UpdateCoyoteTimeCounter()
    {
        if (playerHelpers.IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void OnJump(InputValue value)
    {
        bool buttonDown = value.Get<float>() == 1;

        if (buttonDown && coyoteTimeCounter > 0f)
        {
            Jump();
        }
        else if (buttonDown && playerHelpers.IsAlmostGrounded())
        {
            jumpQueued = true;
            Invoke("ClearJumpQueued", queueDuration);
        }
        else if (!buttonDown && rb.velocity.y > 0.1f && !playerWallJumping.isWallJumping)
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
        OnJumpTriggered?.Invoke();
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
}
