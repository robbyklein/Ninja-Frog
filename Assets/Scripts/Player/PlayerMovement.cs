using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Other components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerSpriteState playerSpriteState;
    [SerializeField] PlayerJumping playerJumping;
    [SerializeField] PlayerWallJumping playerWallJumping;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerHelpers playerHelpers;

    // Settings
    [SerializeField] private float maxMovementSpeed = 20f;
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float decelerationSpeed = 10f;
    [SerializeField] private float accelerator = 1.2f;
    [SerializeField] float slideSpeed = 6f;

    public Vector2 MovementInput { get; private set; }
    public bool IsFacingRight { get; private set; } = true;

    // Events
    public event Action<bool> OnPlayerTurn;

    void Update() {
        if (
            rb.velocity.y < cameraManager.fallSpeedYDampingChangeThreshold &&
            !cameraManager.IsLerpingYDamping &&
            !cameraManager.LerpedFromPlayerFalling
        ) {
            cameraManager.LerpYDamping(true);
        }

        if (
            rb.velocity.y >= 0f &&
            !cameraManager.IsLerpingYDamping &&
            cameraManager.LerpedFromPlayerFalling
        ) {
            cameraManager.LerpedFromPlayerFalling = false;
            cameraManager.LerpYDamping(false);
        }
    }

    void FixedUpdate() {
        SetMaxVelocity();
        Move();
        Slide();
    }

    void OnEnable() {
        playerInput.OnMovementChange += HandleMovement;
    }

    void OnDisable() {
        playerInput.OnMovementChange -= HandleMovement;
    }

    void HandleMovement(Vector2 newMovementInput) {
        if (newMovementInput.x != 0f) {
            bool newIsFacingRight = newMovementInput.x > 0f;

            if (newIsFacingRight != IsFacingRight) {
                IsFacingRight = newIsFacingRight;
                Turn();
                OnPlayerTurn?.Invoke(IsFacingRight);
            }
        }

        MovementInput = newMovementInput;
    }

    void SetMaxVelocity() {
        float max = rb.velocity.y < 0f ? 30f : 35f;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, max);
    }

    void Move() {
        if (!playerWallJumping.IsWallJumping) {
            // Calculate force
            float targetSpeed = MovementInput.x * maxMovementSpeed; // 20, -20, 0
            float speedDifference = targetSpeed - rb.velocity.x; // 10
            float changeRate = Mathf.Abs(targetSpeed) > 0.1f ? accelerationSpeed : decelerationSpeed;
            float horizontalForce = Mathf.Pow(Mathf.Abs(speedDifference) * changeRate, accelerator) * Mathf.Sign(speedDifference);
            Vector2 force = new Vector2(horizontalForce, rb.velocity.y) * Vector2.right;

            // Add it
            rb.AddForce(force * Time.deltaTime);
        }

    }

    void Turn() {
        if (!IsFacingRight) {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        } else {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    void Slide() {
        if (playerHelpers.IsWalled() && !playerWallJumping.IsWallJumping) {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }
}
