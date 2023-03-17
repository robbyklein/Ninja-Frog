using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Store only class instance
    public static CameraManager instance;

    // Settings
    [SerializeField] CinemachineVirtualCamera[] allVirtualCameras;
    [SerializeField] float fallPanAmount = 0.25f;
    [SerializeField] float fallYPanTime = 0.35f;
    [SerializeField] public float fallSpeedYDampingChangeThreshold = -10f;

    // State
    bool isLerpingYDamping;
    bool lerpedFromPlayerFalling;

    Coroutine lerpYPanCoroutine;
    CinemachineFramingTransposer framingTransposer;
    CinemachineVirtualCamera currentCamera;
    float normYPanAmount;

    void Awake()
    {
        // Make singleton
        if (instance == null) instance = this;

        // Find the active camera
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i];
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        // Store initial pan amaount for reverting
        normYPanAmount = framingTransposer.m_YDamping;
    }

    public bool IsLerpingYDamping
    {
        get
        {
            return isLerpingYDamping;
        }
        private set
        {
            isLerpingYDamping = value;
        }
    }

    public bool LerpedFromPlayerFalling
    {
        get
        {
            return lerpedFromPlayerFalling;
        }
        set
        {
            lerpedFromPlayerFalling = value;
        }
    }

    // Runs the coroutine
    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    // Shifts the camera smoothly / NFI
    IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallYPanTime));
            framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }
}
