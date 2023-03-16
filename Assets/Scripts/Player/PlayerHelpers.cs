using UnityEngine;

public class PlayerHelpers : MonoBehaviour
{
    // Other components
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private PlayerMovement playerMovement;
    private PlayerTurning playerTurning;

    // Settings
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWalls;
    [SerializeField] private const float boxCastDistance = .1f;
    [SerializeField] private const float almostBoxCastDistance = 1.5f;
    [SerializeField] private const float closestBoxCastDistance = 3f;
    [SerializeField] float walledVelocityThreshold = 7f;
    private float boxCastAngle = 0f;

    // Types
    public enum WallDirection
    {
        Left,
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerTurning = GetComponent<PlayerTurning>();
    }

    public WallDirection ClosestWallDirection()
    {
        bool closestWallRight = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.right, closestBoxCastDistance, jumpableWalls);
        return closestWallRight ? WallDirection.Right : WallDirection.Left;
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
        Vector2 direction = !playerTurning.isFacingRight ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, boxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && playerMovement.movementInput.x != 0f && rb.velocity.y < walledVelocityThreshold;
    }

    public bool IsAlmostWalled()
    {
        Vector2 direction = !playerTurning.isFacingRight ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, almostBoxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && playerMovement.movementInput.x != 0f;
    }
}
