using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // Components
    private Animator anim;
    private Rigidbody2D rb;

    // Settings
    [SerializeField] private AudioSource deathSound;

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

        // Play death sound effect
        deathSound.Play();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
