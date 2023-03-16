using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // Components
    private Animator anim;
    private Rigidbody2D rb;

    // Event
    public event Action OnDeath;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kills"))
        {
            Die();
        }
    }

    private void Die()
    {
        // Freeze movement
        rb.bodyType = RigidbodyType2D.Static;

        // Play death animation
        anim.SetTrigger("death");

        // Broadcast event
        OnDeath?.Invoke();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
