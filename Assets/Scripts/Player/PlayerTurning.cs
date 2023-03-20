using UnityEngine;

public class PlayerTurning : MonoBehaviour {
    // Other components
    [SerializeField] PlayerMovement playerMovement;

    // Settings
    [SerializeField] CameraFollowObject cameraFollowObject;

    void OnEnable() {
        playerMovement.OnPlayerTurn += Turn;
    }

    void OnDisable() {
        playerMovement.OnPlayerTurn += Turn;
    }

    void Turn(bool isFacingRight) {
        if (!isFacingRight) {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        } else {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }
}
