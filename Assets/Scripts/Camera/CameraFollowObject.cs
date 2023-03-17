using UnityEngine;

public class CameraFollowObject : MonoBehaviour {
    [SerializeField] float flipYRotationTime = 2f;
    [SerializeField] Transform playerTransform;
    [SerializeField] PlayerSpriteState playerSpriteState;
    [SerializeField] PlayerTurning playerTurning;

    void OnEnable() {
        playerTurning.OnTurn += CallTurn;
    }

    void OnDisable() {
        playerTurning.OnTurn -= CallTurn;
    }

    void Update() {
        transform.position = playerTransform.position;
    }

    void CallTurn() {
        // Smooth turn camera pan
        float endRotation = playerTurning.IsFacingRight ? 0f : 180f;
        LeanTween.rotateY(gameObject, endRotation, flipYRotationTime).setEaseInOutSine();
    }
}
