using UnityEngine;

public class PlayerSliding : MonoBehaviour {
    // Other components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerWallJumping playerWallJumping;
    [SerializeField] PlayerHelpers playerHelpers;

    // Settings
    [SerializeField] float slideSpeed = 6f;

    void FixedUpdate() {
        HandleSliding();
    }

    void HandleSliding() {
        if (playerHelpers.IsWalled() && !playerWallJumping.IsWallJumping) {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }
}
