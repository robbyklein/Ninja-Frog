using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour {
    // Components
    [SerializeField] TextMeshProUGUI fpsText;

    // State
    float frameCount = 0;
    double nextUpdate = 0.0;
    double fps = 0.0;
    double updateRate = 4.0;

    void Start() {
        nextUpdate = Time.time;
    }

    void Update() {
        frameCount++;

        if (Time.time > nextUpdate) {
            nextUpdate += 1.0 / updateRate;
            fps = frameCount * updateRate;
            frameCount = 0;
            fpsText.text = "FPS: " + fps;
        }
    }
}
