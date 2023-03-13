using UnityEngine;

public class PlayerSpriteState : MonoBehaviour
{
    // Other components
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private PlayerMovement playerMovement;

    // Settings
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWalls;
    [SerializeField] private CameraFollowObject cameraFollowObject;
    [SerializeField] private const float boxCastDistance = .1f;
    [SerializeField] private const float almostBoxCastDistance = 1.5f;
    [SerializeField] float walledVelocityThreshold = 7f;
    private float boxCastAngle = 0f;

    // State
    public bool isFacingRight = true;
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
    }

    private void FixedUpdate()
    {
        if (playerMovement.movementInput.x > 0f || playerMovement.movementInput.x < 0f)
        {
            TurnCheck();
        }

        UpdateAnimationState();
    }

    private void TurnCheck()
    {
        if (playerMovement.movementInput.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (playerMovement.movementInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            // Turn follow object also
            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            // Turn follow object also
            cameraFollowObject.CallTurn();
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.down, boxCastDistance, jumpableGround);
    }

    public bool IsAlmostGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.down, almostBoxCastDistance, jumpableGround);
    }

    public bool IsWalled()
    {
        Vector2 direction = !isFacingRight ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, boxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && playerMovement.movementInput.x != 0f && rb.velocity.y < walledVelocityThreshold;
    }

    public bool IsAlmostWalled()
    {
        Vector2 direction = !isFacingRight ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, almostBoxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && playerMovement.movementInput.x != 0f;
    }

    private void UpdateAnimationState()
    {
        // Animation state
        MovementState state = MovementState.Idle;

        if (IsWalled())
        {
            state = MovementState.Sliding;
        }
        else if (IsGrounded() && playerMovement.movementInput.x != 0f)
        {
            state = MovementState.Running;
        }
        else if (!IsGrounded() && !IsWalled() && rb.velocity.y > 0.1f)
        {
            state = MovementState.Jumping;
        }
        else if (!IsGrounded() && !IsWalled() && rb.velocity.y < 0.1f)
        {
            state = MovementState.Falling;
        }

        anim.SetInteger("state", (int)state);
    }
}
