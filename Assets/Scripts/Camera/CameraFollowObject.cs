using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private float flipYRotationTime = 2f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerSpriteState playerSpriteState;
    [SerializeField] private PlayerTurning playerTurning;

    private void Start()
    {
        playerTurning.OnTurn += CallTurn;

    }

    // Update is called once per frame
    void Update()
    {
        // Keep in sync with player
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        // Smooth turn camera pan
        float endRotation = playerTurning.isFacingRight ? 0f : 180f;
        LeanTween.rotateY(gameObject, endRotation, flipYRotationTime).setEaseInOutSine();
    }
}
