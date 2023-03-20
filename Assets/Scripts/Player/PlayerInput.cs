using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    // Inputs
    InputActions Input;

    // Events
    public event Action<Vector2> OnMovementChange;
    public event Action OnJump;
    public event Action OnJumpRelease;

    void Awake() {
        Input = new InputActions();
    }

    void OnEnable() {
        // Movement
        Input.Player.Movement.performed += MovementChanged;
        Input.Player.Movement.canceled += MovementChanged;
        Input.Player.Movement.Enable();

        // Jumping
        Input.Player.Jump.performed += JumpPressed;
        Input.Player.Jump.canceled += JumpReleased;
        Input.Player.Jump.Enable();
    }

    void MovementChanged(InputAction.CallbackContext obj) {
        OnMovementChange?.Invoke(obj.ReadValue<Vector2>());
    }

    void JumpPressed(InputAction.CallbackContext obj) {
        OnJump?.Invoke();
    }

    void JumpReleased(InputAction.CallbackContext obj) {
        OnJumpRelease?.Invoke();
    }
}
