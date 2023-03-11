using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private BoxCollider2D coll;
    private PlayerMovement playerMovement;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWalls;

    private const float boxCastAngle = 0f;
    private const float boxCastDistance = .1f;
    private const float almostBoxCastDistance = 1.5f;
    private const float walledThreshold = 7f;
    public bool isFacingRight = true;

    private enum MovementState
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Sliding
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = rb.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        TurnCheck();
        UpdateAnimationState();
    }

    // Helpers
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
        return onWall && !IsGrounded() && playerMovement.movementInput.x != 0f && rb.velocity.y < walledThreshold; // Grounded takes priority
    }

    public bool IsAlmostWalled()
    {
        Vector2 direction = !isFacingRight ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, almostBoxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && playerMovement.movementInput.x != 0f; // Grounded takes priority
    }



    // Sprite state
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
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
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
