using System;
using UnityEngine;

public class PlayerTurning : MonoBehaviour
{
    // Other components
    private PlayerMovement playerMovement;

    // Settings
    [SerializeField] private CameraFollowObject cameraFollowObject;

    // State
    public bool isFacingRight = true;

    // Event
    public event Action OnTurn;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (playerMovement.movementInput.x > 0f || playerMovement.movementInput.x < 0f)
        {
            TurnCheck();
        }
    }

    private void TurnCheck()
    {
        if (playerMovement.movementInput.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (playerMovement.movementInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            // Turn follow object also
            //cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            // Turn follow object also
            //cameraFollowObject.CallTurn();
        }

        OnTurn?.Invoke();
    }
}
