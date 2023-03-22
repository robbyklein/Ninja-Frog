using UnityEngine;

public class SawRotation : MonoBehaviour {
    [SerializeField] float Speed = 3f;

    void Update() {
        transform.Rotate(0, 0, -360 * Speed * Time.deltaTime);
    }
}
