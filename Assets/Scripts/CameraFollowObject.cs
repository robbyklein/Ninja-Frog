using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipYRotationTime = 2f;

    private PlayerMovement playerMovement;
    [SerializeField] private PlayerState playerState;


    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        float endRotation = playerState.isFacingRight ? 0f : 180f;
        LeanTween.rotateY(gameObject, endRotation, flipYRotationTime).setEaseInOutSine();
    }
}
