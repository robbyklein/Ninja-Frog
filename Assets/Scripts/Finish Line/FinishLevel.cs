using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour {
    // Settings
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject finishObject;

    // State
    bool levelCompleted = false;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == finishObject.name && !levelCompleted) {
            levelCompleted = true;

            AudioClip clip = audioManager.FindSound("CollectSound");
            audioSource.clip = clip;
            audioSource.Play();

            float delay = clip == null ? 1f : clip.length + 0.5f;

            //Invoke("CompleteLevel", delay);
        }
    }

    void CompleteLevel() {
        // Restart for now
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
