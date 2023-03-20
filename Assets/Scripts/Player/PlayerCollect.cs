using System;
using UnityEngine;

public class PlayerCollect : MonoBehaviour {
    public event Action<string> OnTrackCollected;

    void OnTriggerEnter2D(Collider2D collision) {
        string tag = collision.gameObject.tag;

        if (tag.Contains("Track")) {
            OnTrackCollected?.Invoke(tag);
            Destroy(collision.gameObject);
        }
    }


}



