using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private BoxCollider2D coll;

    private float dirX;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float boxCastAngle = 0f;
    private float boxCastDistance = .1f;

    [SerializeField] private float jumpForce = 21f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15f, 21f);
    [SerializeField] private float speed = 9f;
    [SerializeField] private float slideSpeed = 6f;
    [SerializeField] private float wallJumpDuration = .2f;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWalls;

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
        sprite = rb.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleRunning();
        HandleSliding();

        UpdateFlipState();
        UpdateAnimationState();
    }

    private void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        dirX = movement.x;
    }

    private void OnJump(InputValue value)
    {
        HandleJumping();
        HandleWallJumping();
    }

    private void HandleRunning()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        }
    }

    private void HandleJumping()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(-dirX * rb.velocity.x, jumpForce);
        }
    }

    private void HandleSliding()
    {
        if (IsWalled() && !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }

    private void HandleWallJumping()
    {
        // Wall Jump
        if (IsWalled())
        {
            // Set delay before user gets control again
            wallJumpDirection = -dirX;
            isWallJumping = true;
            Invoke("FinishWallJump", wallJumpDuration);
        }

        if (isWallJumping)
        {
            // Jump out and up
            rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
        }
    }

    private void FinishWallJump()
    {
        isWallJumping = false;
    }

    private void UpdateFlipState()
    {
        sprite.flipX = dirX < 0f;
    }

    private void UpdateAnimationState()
    {
        // Animation state
        MovementState state = MovementState.Idle;

        if (IsWalled())
        {
            state = MovementState.Sliding;
        }
        else if (IsGrounded() && dirX != 0f)
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

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.down, boxCastDistance, jumpableGround);
    }

    private bool IsWalled()
    {
        Vector2 direction = sprite.flipX ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, boxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && dirX != 0f; // Grounded takes priority
    }
}
