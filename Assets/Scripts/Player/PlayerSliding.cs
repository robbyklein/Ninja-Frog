using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    // Other components
    private Rigidbody2D rb;
    private PlayerSpriteState playerSpriteState;
    private PlayerWallJumping playerWallJumping;

    // Settings
    [SerializeField] private float slideSpeed = 6f;

    // State
    public bool lastSlideRight { get; set; } = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSpriteState = GetComponent<PlayerSpriteState>();
        playerWallJumping = GetComponent<PlayerWallJumping>();
    }

    private void FixedUpdate()
    {
        HandleSliding();
    }

    private void HandleSliding()
    {
        if (playerSpriteState.IsWalled() && !playerWallJumping.isWallJumping)
        {
            lastSlideRight = playerSpriteState.isFacingRight;
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }
}
