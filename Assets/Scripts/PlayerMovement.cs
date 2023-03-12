using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private PlayerState playerState;
    private PlayerJumping playerJumping;

    // Movement
    public Vector2 movementInput;
    [SerializeField] private float slideSpeed = 6f;
    [SerializeField] private float maxMovementSpeed = 20f;
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float decelerationSpeed = 10f;
    [SerializeField] private float accelerator = 1.2f;

    // Camera
    private float fallSpeedYDampingChangeThreshold;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        playerJumping = GetComponent<PlayerJumping>();
        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;

        //Physics2D.IgnoreLayerCollision(0, 8);
    }

    private void FixedUpdate()
    {
        SetMaxVelocity();
        Move();
        HandleSliding();
    }

    void Update()
    {
        if (rb.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if (rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void SetMaxVelocity()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, 30f);
    }

    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void Move()
    {
        if (!playerJumping.isWallJumping)
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

    // Wall sliding
    private void HandleSliding()
    {
        if (playerState.IsWalled() && !playerJumping.isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }
}
