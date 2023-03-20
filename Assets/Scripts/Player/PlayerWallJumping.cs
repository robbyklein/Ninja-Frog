using System;
using UnityEngine;


public class PlayerWallJumping : MonoBehaviour {
    // Components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerHelpers playerHelpers;
    [SerializeField] PlayerInput playerInput;

    // Settings
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15f, 21f);
    [SerializeField] private float wallJumpDuration = .2f;
    [SerializeField] private float queueDuration = 0.15f;
    [SerializeField] private float coyoteTime = 0.1f;

    // State
    float CoyoteTimeCounter;
    bool WallJumpQueued = false;
    public bool IsWallJumping { get; private set; }

    // Event
    public event Action OnWallJumpTriggered;

    void OnEnable() {
        playerInput.OnJump += OnJump;
    }

    void OnDisable() {
        playerInput.OnJump -= OnJump;
    }

    void Update() {
        UpdateCoyoteTimeCounter();
        HandleQueuedWallJumps();
    }

    void OnJump() {
        if (CoyoteTimeCounter > 0f) {
            WallJump();
        } else if (playerHelpers.IsAlmostWalled()) {
            WallJumpQueued = true;
            Invoke("ClearWallJumpQueued", queueDuration);
        }
    }

    void UpdateCoyoteTimeCounter() {
        if (playerHelpers.IsWalled()) {
            CoyoteTimeCounter = coyoteTime;
        } else {
            CoyoteTimeCounter -= Time.deltaTime;
        }
    }

    void WallJump() {
        // Set local state
        IsWallJumping = true;

        // Call clear local state function
        Invoke("FinishWallJump", wallJumpDuration);

        // Broadcast event
        OnWallJumpTriggered?.Invoke();

        // Figure out direction
        PlayerHelpers.WallDirection closestWall = playerHelpers.ClosestWallDirection();
        float xDirection = closestWall == PlayerHelpers.WallDirection.Right ? -1f : 1f;

        // Perform movement
        rb.velocity = new Vector2(xDirection * wallJumpForce.x, wallJumpForce.y);
    }

    void HandleQueuedWallJumps() {
        if (WallJumpQueued && CoyoteTimeCounter > 0f) {
            ClearWallJumpQueued();
            WallJump();
        }
    }

    void FinishWallJump() {
        // Update local state
        IsWallJumping = false;
    }

    void ClearWallJumpQueued() {
        WallJumpQueued = false;
    }
}
