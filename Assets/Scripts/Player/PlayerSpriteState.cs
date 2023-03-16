using UnityEngine;

public class PlayerSpriteState : MonoBehaviour
{
    // Other components
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private PlayerMovement playerMovement;
    private PlayerHelpers playerHelpers;

    // Settings

    [SerializeField] private CameraFollowObject cameraFollowObject;

    private enum MovementState
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Sliding
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHelpers = GetComponent<PlayerHelpers>();
    }

    private void FixedUpdate()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        // Animation state
        MovementState state = MovementState.Idle;

        bool isWalled = playerHelpers.IsWalled();
        bool isGrounded = playerHelpers.IsGrounded();

        if (isWalled)
        {
            state = MovementState.Sliding;
        }
        else if (isGrounded && playerMovement.movementInput.x != 0f)
        {
            state = MovementState.Running;
        }
        else if (!isGrounded && !isWalled && rb.velocity.y > 0.1f)
        {
            state = MovementState.Jumping;
        }
        else if (!isGrounded && !isWalled && rb.velocity.y < 0.1f)
        {
            state = MovementState.Falling;
        }

        anim.SetInteger("state", (int)state);
    }
}
