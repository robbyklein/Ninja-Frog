using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private PlayerJumping playerJumping;
    private PlayerWallJumping playerWallJumping;
    private PlayerDeath playerDeath;

    private AudioSource audioSource;

    // Settings
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip collectSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerJumping = GetComponent<PlayerJumping>();
        playerWallJumping = GetComponent<PlayerWallJumping>();
        playerDeath = GetComponent<PlayerDeath>();

        // Subscribe to events
        playerJumping.OnJumpTriggered += PlayJumpSound;
        playerWallJumping.OnWallJumpTriggered += PlayJumpSound;
        playerDeath.OnDeath += PlayDeathSound;

    }

    // Update is called once per frame
    void PlayJumpSound()
    {
        audioSource.clip = jumpSound;
        audioSource.Play();
    }

    void PlayDeathSound()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
    }
}
