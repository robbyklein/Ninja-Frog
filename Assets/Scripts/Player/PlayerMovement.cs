using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Other components
    private Rigidbody2D rb;
    private PlayerSpriteState playerSpriteState;
    private PlayerJumping playerJumping;
    private PlayerWallJumping playerWallJumping;

    // Settings
    [SerializeField] private float maxMovementSpeed = 20f;
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float decelerationSpeed = 10f;
    [SerializeField] private float accelerator = 1.2f;

    // Input state
    public Vector2 movementInput { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSpriteState = GetComponent<PlayerSpriteState>();
        playerJumping = GetComponent<PlayerJumping>();
        playerWallJumping = GetComponent<PlayerWallJumping>();
    }

    private void FixedUpdate()
    {
        SetMaxVelocity();
        Move();
    }

    void Update()
    {
        CameraManager cm = CameraManager.instance;

        if (rb.velocity.y < cm.fallSpeedYDampingChangeThreshold && !cm.IsLerpingYDamping && !cm.LerpedFromPlayerFalling)
        {
            cm.LerpYDamping(true);
        }

        if (rb.velocity.y >= 0f && !cm.IsLerpingYDamping && cm.LerpedFromPlayerFalling)
        {
            cm.LerpedFromPlayerFalling = false;
            cm.LerpYDamping(false);
        }
    }

    private void SetMaxVelocity()
    {
        float max = rb.velocity.y < 0f ? 30f : 35f;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, max);
    }

    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void Move()
    {
        if (!playerWallJumping.isWallJumping)
        {
            // Calculate force
            float targetSpeed = movementInput.x * maxMovementSpeed; // 20, -20, 0
            float speedDifference = targetSpeed - rb.velocity.x; // 10
            float changeRate = Mathf.Abs(targetSpeed) > 0.1f ? accelerationSpeed : decelerationSpeed;
            float horizontalForce = Mathf.Pow(Mathf.Abs(speedDifference) * changeRate, accelerator) * Mathf.Sign(speedDifference);
            Vector2 force = new Vector2(horizontalForce, rb.velocity.y) * Vector2.right;

            // Add it
            rb.AddForce(force * Time.deltaTime);
        }

    }
}
