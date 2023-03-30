using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour {
    // Components
    [SerializeField] PlayerInputManager playerInput;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;

    // Event
    public event Action OnDeath;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Kills")) {
            Die();
        }
    }

    void Die() {
        // Freeze movement
        rb.bodyType = RigidbodyType2D.Static;

        // Play death animation
        anim.SetTrigger("death");

        // Broadcast event
        OnDeath?.Invoke();
    }

    void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
