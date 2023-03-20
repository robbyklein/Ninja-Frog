using UnityEngine;

public class PlayerHelpers : MonoBehaviour {
    // Other components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] BoxCollider2D coll;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerTurning playerTurning;

    // Settings
    [SerializeField] LayerMask jumpableGround;
    [SerializeField] LayerMask jumpableWalls;
    [SerializeField] const float boxCastDistance = .1f;
    [SerializeField] const float almostBoxCastDistance = 1.5f;
    [SerializeField] const float closestBoxCastDistance = 3f;
    [SerializeField] float walledVelocityThreshold = 7f;
    static float boxCastAngle = 0f;

    Vector2 movementInput;

    void OnEnable() {
        //subscribe
    }

    void setIt(Vector2 move) {
        movementInput = move;
    }

    // Types
    public enum WallDirection {
        Left,
        Right
    }

    public WallDirection ClosestWallDirection() {
        bool closestWallRight = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.right, closestBoxCastDistance, jumpableWalls);
        return closestWallRight ? WallDirection.Right : WallDirection.Left;
    }

    public bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.down, boxCastDistance, jumpableGround);
    }

    public bool IsAlmostGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, Vector2.down, almostBoxCastDistance, jumpableGround);
    }

    public bool IsWalled() {
        Vector2 direction = !playerMovement.IsFacingRight ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, boxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && playerMovement.MovementInput.x != 0f && rb.velocity.y < walledVelocityThreshold;
    }

    public bool IsAlmostWalled() {
        Vector2 direction = !playerMovement.IsFacingRight ? Vector2.left : Vector2.right;
        bool onWall = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, boxCastAngle, direction, almostBoxCastDistance, jumpableWalls);
        return onWall && !IsGrounded() && playerMovement.MovementInput.x != 0f;
    }
}
