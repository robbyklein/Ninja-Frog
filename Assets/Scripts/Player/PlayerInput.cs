using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    // Inputs
    public InputActions Input { get; private set; }

    // Events
    public event Action<InputActionMap> OnActionMapChanged;
    public event Action<Vector2> OnMovementChange;
    public event Action OnJumpPress;
    public event Action OnJumpRelease;
    public event Action OnStartPress;

    public event Action<Vector2> OnMenusMovementChanged;
    public event Action OnMenusClosePress;

    void Awake() {
        Input = new InputActions();
    }

    void OnEnable() {
        // Player Movement
        Input.Player.Movement.performed += MovementChanged;
        Input.Player.Movement.canceled += MovementChanged;
        Input.Player.Movement.Enable();

        Input.Player.Jump.performed += JumpPressed;
        Input.Player.Jump.canceled += JumpReleased;
        Input.Player.Jump.Enable();

        Input.Player.Start.performed += StartPressed;
        Input.Player.Start.Enable();


        // Menus
        Input.Menus.Movement.performed += MenusMovementChanged;
        Input.Menus.Close.performed += MenusClosePressed;
    }

    void MovementChanged(InputAction.CallbackContext obj) {
        Debug.Log("eyyyy");
        OnMovementChange?.Invoke(obj.ReadValue<Vector2>());
    }

    void JumpPressed(InputAction.CallbackContext obj) {
        OnJumpPress?.Invoke();
    }

    void JumpReleased(InputAction.CallbackContext obj) {
        OnJumpRelease?.Invoke();
    }

    void StartPressed(InputAction.CallbackContext obj) {
        OnStartPress?.Invoke();
    }

    void MenusMovementChanged(InputAction.CallbackContext obj) {
        OnMenusMovementChanged?.Invoke(obj.ReadValue<Vector2>());
    }

    void MenusClosePressed(InputAction.CallbackContext obj) {
        OnMenusClosePress?.Invoke();
    }

    public void ChangeActionMap(InputActionMap actionMap) {
        if (!actionMap.enabled) {
            Input.Disable();
            OnActionMapChanged?.Invoke(actionMap);
            actionMap.Enable();
        }
    }

}
