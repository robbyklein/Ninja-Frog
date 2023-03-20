using UnityEngine;

public class PlayerInput : MonoBehaviour {
    InputActions Inputs;

    Vector2 Movement = Vector2.zero;

    private void Awake() {
        Inputs = new InputActions();
    }

    void OnEnable() {
        Inputs.Player.Movement.Enable();
    }

    void OnDisable() {
        Inputs.Player.Movement.Disable();
    }

    void UpdateMovement() {
    }

}
