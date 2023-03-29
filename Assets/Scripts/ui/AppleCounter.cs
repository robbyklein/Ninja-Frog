using TMPro;
using UnityEngine;

public class AppleCounter : MonoBehaviour {
    int CollectedApples = 0;
    [SerializeField] PlayerCollect pc;
    [SerializeField] TextMeshProUGUI CounterText;

    void OnEnable() {
        pc.OnAppleCollected += HandleAppleCollected;
    }

    void OnDisable() {
        pc.OnAppleCollected -= HandleAppleCollected;
    }

    void HandleAppleCollected() {
        CollectedApples++;
        CounterText.text = $"{CollectedApples}/5";
    }
}
