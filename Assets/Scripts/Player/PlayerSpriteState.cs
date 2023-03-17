using UnityEngine;

public class PlayerSpriteState : MonoBehaviour {
    // Other components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] BoxCollider2D coll;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerHelpers playerHelpers;

    // Settings
    [SerializeField] CameraFollowObject cameraFollowObject;

    enum MovementState {
        Idle,
        Running,
        Jumping,
        Falling,
        Sliding
    }

    void FixedUpdate() {
        UpdateAnimationState();
    }

    void UpdateAnimationState() {
        // Animation state
        MovementState state = MovementState.Idle;

        bool isWalled = playerHelpers.IsWalled();
        bool isGrounded = playerHelpers.IsGrounded();

        if (isWalled) {
            state = MovementState.Sliding;
        } else if (isGrounded && playerMovement.MovementInput.x != 0f) {
            state = MovementState.Running;
        } else if (!isGrounded && !isWalled && rb.velocity.y > 0.1f) {
            state = MovementState.Jumping;
        } else if (!isGrounded && !isWalled && rb.velocity.y < 0.1f) {
            state = MovementState.Falling;
        }

        anim.SetInteger("state", (int)state);
    }
}
