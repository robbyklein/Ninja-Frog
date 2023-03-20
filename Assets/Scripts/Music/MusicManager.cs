using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] AudioSource Track2;
    [SerializeField] AudioSource Track3;
    [SerializeField] AudioSource Track4;
    [SerializeField] AudioSource Track5;
    [SerializeField] PlayerCollect playerCollect;

    // Start is called before the first frame update
    void OnEnable() {
        playerCollect.OnTrackCollected += HandleTrackCollected;
    }

    void OnDisable() {
        playerCollect.OnTrackCollected += HandleTrackCollected;
    }

    void HandleTrackCollected(string track) {
        if (track == "Track2") {
            Track2.volume = 100;
        } else if (track == "Track2") {
            Track2.volume = 100;
        } else if (track == "Track3") {
            Track3.volume = 100;
        } else if (track == "Track4") {
            Track4.volume = 100;
        } else if (track == "Track5") {
            Track5.volume = 100;
        }
    }
}
