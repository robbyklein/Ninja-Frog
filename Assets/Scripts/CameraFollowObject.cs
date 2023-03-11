using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipYRotationTime = 0.5f;

    private PlayerMovement playerMovement;
    private PlayerState playerState;


    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        float endRotation = playerState.isFacingRight ? 180f : 0;
        LeanTween.rotateY(gameObject, endRotation, flipYRotationTime).setEaseInOutSine();
    }
}
