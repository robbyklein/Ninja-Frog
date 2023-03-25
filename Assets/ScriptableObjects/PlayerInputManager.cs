using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Input Manager", menuName = "ScriptableObjects/Managers/PlayerInputManager")]
public class PlayerInputManager : ScriptableObject {
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
    public event Action OnMenusSelectPress;


    void OnEnable() {
        // If null assign
        Input ??= new InputActions();

        // Player Movement
        Input.Player.Movement.performed += MovementChanged;
        Input.Player.Movement.canceled += MovementChanged;
        Input.Player.Jump.performed += JumpPressed;
        Input.Player.Jump.canceled += JumpReleased;
        Input.Player.Start.performed += StartPressed;
        Input.Player.Enable();


        // Menus
        Input.Menus.Movement.performed += MenusMovementChanged;
        Input.Menus.Close.performed += MenusClosePressed;
        Input.Menus.Select.performed += MenusSelectPressed;
    }

    void OnDisable() {
        Input.Player.Movement.performed -= MovementChanged;
        Input.Player.Movement.canceled -= MovementChanged;
        Input.Player.Jump.performed -= JumpPressed;
        Input.Player.Jump.canceled -= JumpReleased;
        Input.Player.Start.performed -= StartPressed;

        Input.Menus.Movement.performed -= MenusMovementChanged;
        Input.Menus.Close.performed -= MenusClosePressed;
        Input.Menus.Select.performed -= MenusSelectPressed;
    }

    void MovementChanged(InputAction.CallbackContext obj) {
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

    void MenusSelectPressed(InputAction.CallbackContext obj) {
        OnMenusSelectPress?.Invoke();
    }

    public void ChangeActionMap(InputActionMap actionMap) {
        if (!actionMap.enabled) {
            Input.Disable();
            OnActionMapChanged?.Invoke(actionMap);
            actionMap.Enable();
        }
    }

}
