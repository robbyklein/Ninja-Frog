using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform subject;

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(subject.position.x, subject.position.y, transform.position.z);
    }
}
