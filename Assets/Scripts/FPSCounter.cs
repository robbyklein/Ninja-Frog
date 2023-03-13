using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    // Components
    private TextMeshProUGUI fpsText;

    // State
    private float frameCount = 0;
    private double nextUpdate = 0.0;
    private double fps = 0.0;
    private double updateRate = 4.0;

    void Start()
    {
        nextUpdate = Time.time;
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        frameCount++;

        if (Time.time > nextUpdate)
        {
            nextUpdate += 1.0 / updateRate;
            fps = frameCount * updateRate;
            frameCount = 0;
            fpsText.text = "FPS: " + fps;
        }
    }
}
