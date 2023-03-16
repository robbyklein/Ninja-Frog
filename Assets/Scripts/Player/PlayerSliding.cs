using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    // Other components
    private Rigidbody2D rb;
    private PlayerWallJumping playerWallJumping;
    private PlayerHelpers playerHelpers;
    private PlayerTurning playerTurning;

    // Settings
    [SerializeField] private float slideSpeed = 6f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerWallJumping = GetComponent<PlayerWallJumping>();
        playerHelpers = GetComponent<PlayerHelpers>();
        playerTurning = GetComponent<PlayerTurning>();
    }

    private void FixedUpdate()
    {
        HandleSliding();
    }

    private void HandleSliding()
    {
        if (playerHelpers.IsWalled() && !playerWallJumping.isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }
}
