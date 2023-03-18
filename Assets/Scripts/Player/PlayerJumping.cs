using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumping : MonoBehaviour {
    // Components
    [SerializeField] PlayerSpriteState playerSpriteState;
    [SerializeField] PlayerWallJumping playerWallJumping;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerHelpers playerHelpers;

    // Settings
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float queueDuration = 0.15f;
    [SerializeField] float jumpForce = 21f;

    // State
    public float CoyoteTimeCounter { get; private set; }
    public bool JumpQueued { get; private set; } = false;

    // Event
    public event Action OnJumpTriggered;

    void Update() {
        UpdateCoyoteTimeCounter();
        HandleQueuedJumps();
    }

    void UpdateCoyoteTimeCounter() {
        if (playerHelpers.IsGrounded()) {
            CoyoteTimeCounter = coyoteTime;
        } else {
            CoyoteTimeCounter -= Time.deltaTime;
        }
    }

    void OnJump(InputValue value) {
        bool buttonDown = value.Get<float>() == 1;

        if (buttonDown && CoyoteTimeCounter > 0f) {
            Jump();
        } else if (buttonDown && playerHelpers.IsAlmostGrounded()) {
            JumpQueued = true;
            Invoke("ClearJumpQueued", queueDuration);
        } else if (!buttonDown && rb.velocity.y > 0.1f && !playerWallJumping.IsWallJumping) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            CoyoteTimeCounter = 0f;
        }
    }

    void Jump() {
        // Perform jump
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // Play sound effect
        OnJumpTriggered?.Invoke();
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
