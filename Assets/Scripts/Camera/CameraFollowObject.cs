using UnityEngine;

public class CameraFollowObject : MonoBehaviour {
    [SerializeField] float flipYRotationTime = 2f;
    [SerializeField] Transform playerTransform;
    [SerializeField] PlayerSpriteState playerSpriteState;
    [SerializeField] PlayerMovement playerMovement;

    void OnEnable() {
        playerMovement.OnPlayerTurn += CallTurn;
    }

    void OnDisable() {
        playerMovement.OnPlayerTurn -= CallTurn;
    }

    void Update() {
        transform.position = playerTransform.position;
    }

    void CallTurn(bool isFacingRight) {
        // Smooth turn camera pan
        float endRotation = isFacingRight ? 0f : 180f;
        LeanTween.rotateY(gameObject, endRotation, flipYRotationTime).setEaseInOutSine();
    }
}
