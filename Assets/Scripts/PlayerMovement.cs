using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private BoxCollider2D coll;

    private float dirX;
    private bool isWallJumping;
    private float wallJumpDirection;

    [SerializeField] private float jumpForce = 21f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(15f, 21f);
    [SerializeField] private float speed = 9f;
    [SerializeField] private float slideSpeed = 6f;
    [SerializeField] private float wallJumpDuration = .2f;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWalls;

    private enum MovementState
    {
        idle,
        running,
        jumping,
        falling,
        sliding
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
        // Set horizontal input amount
        dirX = Input.GetAxisRaw("Horizontal");

        // Running
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        }

        // Normal Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(-dirX * rb.velocity.x, jumpForce);
        }

        // Wall Jump
        if (Input.GetButtonDown("Jump") && IsWalled())
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


        // Handle Slide
        if (IsWalled() && !isWallJumping)
        {
            Debug.Log("Player Sliding");
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }

        UpdateAnimationState();
    }

    private void FinishWallJump()
    {
        isWallJumping = false;
    }

    private void UpdateAnimationState()
    {
        // Flip state
        sprite.flipX = dirX < 0f;

        // Animation state
        MovementState state = MovementState.idle;

        if (IsWalled())
        {
            state = MovementState.sliding;
        }
        else if (IsGrounded() && dirX != 0f)
        {
            state = MovementState.running;
        }
        else if (!IsGrounded() && !IsWalled() && rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (!IsGrounded() && !IsWalled() && rb.velocity.y < 0.1f)
            state = MovementState.falling;
        {
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private bool IsWalled()
    {
        Vector2 direction = sprite.flipX ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, direction, .1f, jumpableWalls);
        return onWall && !IsGrounded() && dirX != 0f; // Grounded takes priority
    }

}
