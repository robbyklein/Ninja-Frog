using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    // Other components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerSpriteState playerSpriteState;
    [SerializeField] PlayerJumping playerJumping;
    [SerializeField] PlayerWallJumping playerWallJumping;

    // Settings
    [SerializeField] private float maxMovementSpeed = 20f;
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float decelerationSpeed = 10f;
    [SerializeField] private float accelerator = 1.2f;

    // Input state
    public Vector2 MovementInput { get; private set; }

    // Event 
    public event Action<Vector2> OnMovementChange;

    void FixedUpdate() {
        SetMaxVelocity();
        Move();
    }

    void Update() {
        CameraManager cm = CameraManager.instance;

        if (rb.velocity.y < cm.fallSpeedYDampingChangeThreshold && !cm.IsLerpingYDamping && !cm.LerpedFromPlayerFalling) {
            cm.LerpYDamping(true);
        }

        if (rb.velocity.y >= 0f && !cm.IsLerpingYDamping && cm.LerpedFromPlayerFalling) {
            cm.LerpedFromPlayerFalling = false;
            cm.LerpYDamping(false);
        }
    }

    void SetMaxVelocity() {
        float max = rb.velocity.y < 0f ? 30f : 35f;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, max);
    }

    void OnMove(InputValue value) {
        Vector2 newInput = value.Get<Vector2>();

        if (MovementInput != newInput) {
            OnMovementChange?.Invoke(newInput);
        }

        MovementInput = newInput;
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
}
