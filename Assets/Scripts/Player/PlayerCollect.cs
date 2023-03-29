using System;
using UnityEngine;

public class PlayerCollect : MonoBehaviour {
    public event Action OnAppleCollected;

    void OnTriggerEnter2D(Collider2D collision) {
        string tag = collision.gameObject.tag;

        if (tag.Contains("Collectable")) {
            OnAppleCollected?.Invoke();
            Destroy(collision.gameObject);
        }
    }


}



