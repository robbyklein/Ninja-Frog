using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private BoxCollider2D coll;

    // Constants
    private const float boxCastAngle = 0f;
    private const float boxCastDistance = .1f;
    private const float almostBoxCastDistance = 2f;
    private const float walledThreshold = 7f;
    private const float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // Movement
    private Vector2 movementInput;
    [SerializeField] private float maxMovementSpeed = 20f;
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float decelerationSpeed = 10f;
    [SerializeField] private float accelerator = 1.2f;

    // Jumping
    private bool isWallJumping;
    private bool jumpQueued = false;
    private bool wallJumpQueued = false;
    private const float queueDuration = 0.15f;
    [SerializeField] private float jumpForce = 21f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15f, 21f);
    [SerializeField] private float slideSpeed = 6f;
    [SerializeField] private float wallJumpDuration = .2f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWalls;

    // Sounds
    [SerializeField] private AudioSource jumpSound;

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
        HandleCoyoteTime();
        HandleQueuedJumps();
        HandleQueuedWallJumps();
        UpdateFlipState();
        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        SetMaxVelocity();
        Move();
        HandleSliding();
    }

    private void SetMaxVelocity()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, 30f);
    }

    private void HandleCoyoteTime()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    // Movement
    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void Move()
    {
        if (!isWallJumping)
        {
            // Figure out the direction we're moving
            Vector2 direction = movementInput.x < 0f ? Vector2.left : Vector2.right;

            // Calculate force
            float targetSpeed = movementInput.x * maxMovementSpeed; // 20, -20, 0
            float speedDifference = targetSpeed - rb.velocity.x; // 10
            float changeRate = Mathf.Abs(targetSpeed) > 0.1f ? accelerationSpeed : decelerationSpeed;
            float horizontalForce = Mathf.Pow(Mathf.Abs(speedDifference) * changeRate, accelerator) * Mathf.Sign(speedDifference);

            Vector2 force = new Vector2(horizontalForce, rb.velocity.y) * Vector2.right;

            // Add it
            rb.AddForce(force);
        }

    }

    private void OnRun(InputValue value)
    {
        bool buttonDown = value.Get<float>() == 1;

    }

    // Jumping
    private void OnJump(InputValue value)
    {
        bool buttonDown = value.Get<float>() == 1;

        if (buttonDown && coyoteTimeCounter > 0f)
        {
            Jump();
        }
        else if (buttonDown && IsWalled())
        {
            WallJump();
        }
        else if (buttonDown && IsAlmostGrounded())
        {
            jumpQueued = true;
            Invoke("ClearJumpQueued", queueDuration);
        }
        else if (buttonDown && IsAlmostWalled())
        {
            wallJumpQueued = true;
            Invoke("ClearWallJumpQueued", queueDuration);
        }
        else if (!buttonDown && rb.velocity.y > 0.1f && !isWallJumping)
        {
            // Shorten jump early if not already falling
            rb.AddForce(new Vector2(0, rb.velocity.y * 20f * Mathf.Sign(-rb.velocity.y)));
            coyoteTimeCounter = 0f; // Clear late jumping

        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce * 100), ForceMode2D.Impulse);
        jumpSound.Play();
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

    private void WallJump()
    {
        // Set delay before user gets control again
        isWallJumping = true;
        Invoke("FinishWallJump", wallJumpDuration);


        if (isWallJumping)
        {
            // Jump out and up
            jumpSound.Play();
            rb.velocity = new Vector2(-movementInput.x * wallJumpForce.x, wallJumpForce.y);
        }
    }

    private void HandleQueuedWallJumps()
    {
        if (wallJumpQueued && IsWalled())
        {
            ClearWallJumpQueued();
            WallJump();
        }
    }

    private void FinishWallJump()
    {
        isWallJumping = false;
    }

    private void ClearWallJumpQueued()
    {
        wallJumpQueued = false;
    }


    // Wall sliding
    private void HandleSliding()
    {
        if (IsWalled() && !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }

    // Helpers
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.down, boxCastDistance, jumpableGround);
    }

    private bool IsAlmostGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.down, almostBoxCastDistance, jumpableGround);
    }

    private bool IsWalled()
    {
        Vector2 direction = sprite.flipX ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, boxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && movementInput.x != 0f && rb.velocity.y < walledThreshold; // Grounded takes priority
    }

    private bool IsAlmostWalled()
    {
        Vector2 direction = sprite.flipX ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, almostBoxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && movementInput.x != 0f; // Grounded takes priority
    }

    // Sprite state
    private void UpdateFlipState()
    {
        if (movementInput.x < 0f)
        {
            sprite.flipX = true;
        }
        else if (movementInput.x > 0f)
        {
            sprite.flipX = false;
        }
    }

    private void UpdateAnimationState()
    {
        // Animation state
        MovementState state = MovementState.Idle;

        if (IsWalled())
        {
            state = MovementState.Sliding;
        }
        else if (IsGrounded() && movementInput.x != 0f)
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
