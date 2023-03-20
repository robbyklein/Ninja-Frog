using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    #region Fields
    // Other components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerSpriteState playerSpriteState;
    [SerializeField] PlayerJumping playerJumping;
    [SerializeField] PlayerWallJumping playerWallJumping;
    [SerializeField] CameraManager cameraManager;

    // Settings
    [SerializeField] private float maxMovementSpeed = 20f;
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float decelerationSpeed = 10f;
    [SerializeField] private float accelerator = 1.2f;
    #endregion

    // Input state
    InputActions Input;
    public Vector2 MovementInput { get; private set; }
    public bool IsFacingRight { get; private set; } = true;

    // Events
    public event Action<bool> OnPlayerTurn;

    void FixedUpdate() {
        SetMaxVelocity();
        Move();
    }

    void Awake() {
        Input = new InputActions();
    }

    void OnEnable() {
        Input.Player.Movement.performed += MovementInputChanged;
        Input.Player.Movement.canceled += MovementInputChanged;
        Input.Player.Movement.Enable();
    }

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

    void MovementInputChanged(InputAction.CallbackContext obj) {
        Vector2 newMovementInput = obj.ReadValue<Vector2>();

        if (newMovementInput.x != 0f) {
            bool newIsFacingRight = newMovementInput.x > 0f;

            if (newIsFacingRight != IsFacingRight) {
                IsFacingRight = newIsFacingRight;
                OnPlayerTurn?.Invoke(IsFacingRight);
            }
        }

        MovementInput = obj.ReadValue<Vector2>();
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
}
