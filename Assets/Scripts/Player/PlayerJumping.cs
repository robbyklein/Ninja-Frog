using System;
using UnityEngine;

public class PlayerJumping : MonoBehaviour {
    // Components
    [SerializeField] PlayerSpriteState playerSpriteState;
    [SerializeField] PlayerWallJumping playerWallJumping;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerHelpers playerHelpers;
    [SerializeField] PlayerInput playerInput;

    // Settings
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float queueDuration = 0.15f;
    [SerializeField] float jumpForce = 21f;

    // State
    public float CoyoteTimeCounter { get; private set; }
    public bool JumpQueued { get; private set; } = false;

    // Event
    public event Action OnJump;

    void OnEnable() {
        playerInput.OnJumpPress += HandleJumpPress;
        playerInput.OnJumpRelease += HandleJumpRelease;
    }

    void OnDisable() {
        playerInput.OnJumpPress -= HandleJumpPress;
        playerInput.OnJumpRelease -= HandleJumpRelease;
    }

    void Update() {
        UpdateCoyoteTimeCounter();
        HandleQueuedJumps();
    }

    void HandleJumpPress() {
        if (CoyoteTimeCounter > 0f) {
            Jump();
        } else if (playerHelpers.IsAlmostGrounded()) {
            JumpQueued = true;
            Invoke("ClearJumpQueued", queueDuration);
        }
    }

    void HandleJumpRelease() {
        if (rb.velocity.y > 0.1f && !playerWallJumping.IsWallJumping) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            CoyoteTimeCounter = 0f;
        }
    }

    void UpdateCoyoteTimeCounter() {
        if (playerHelpers.IsGrounded()) {
            CoyoteTimeCounter = coyoteTime;
        } else {
            CoyoteTimeCounter -= Time.deltaTime;
        }
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        OnJump?.Invoke();
    }

    void HandleQueuedJumps() {
        if (JumpQueued && CoyoteTimeCounter > 0f) {
            ClearJumpQueued();
            Jump();
        }
    }

    void ClearJumpQueued() {
        JumpQueued = false;
    }
}
