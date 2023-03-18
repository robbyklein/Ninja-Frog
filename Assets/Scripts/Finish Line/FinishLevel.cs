using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour {
    // Settings
    [SerializeField] GameObject finishObject;
    [SerializeField] FinishLevelAudio finishLevelAudio;

    // State
    bool levelCompleted = false;

    // Event
    public event Action OnLevelFinished;

    void OnEnable() {
        finishLevelAudio.OnSoundFinished += CompleteLevel;
    }

    void OnDisble() {
        finishLevelAudio.OnSoundFinished -= CompleteLevel;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == finishObject.name && !levelCompleted) {
            levelCompleted = true;
            OnLevelFinished?.Invoke();
        }
    }

    void CompleteLevel() {
        // Restart for now
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
