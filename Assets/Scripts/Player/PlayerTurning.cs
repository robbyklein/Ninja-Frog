using System;
using UnityEngine;

public class PlayerTurning : MonoBehaviour {
    // Other components
    [SerializeField] PlayerMovement playerMovement;

    // Settings
    [SerializeField] CameraFollowObject cameraFollowObject;

    // State
    public bool IsFacingRight { get; private set; } = true;

    // Event
    public event Action OnTurn;

    void FixedUpdate() {
        if (playerMovement.MovementInput.x > 0f || playerMovement.MovementInput.x < 0f) {
            TurnCheck();
        }
    }

    void TurnCheck() {
        if (playerMovement.MovementInput.x > 0 && !IsFacingRight) {
            Turn();
        } else if (playerMovement.MovementInput.x < 0 && IsFacingRight) {
            Turn();
        }
    }

    void Turn() {
        if (IsFacingRight) {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        } else {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }

        OnTurn?.Invoke();
    }
}
