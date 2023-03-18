using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    // Settings
    [SerializeField] PlayerJumping playerJumping;
    [SerializeField] PlayerWallJumping playerWallJumping;
    [SerializeField] PlayerDeath playerDeath;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioManager sounds;

    void OnEnable() {
        playerJumping.OnJumpTriggered += PlayJumpSound;
        playerWallJumping.OnWallJumpTriggered += PlayJumpSound;
        playerDeath.OnDeath += PlayDeathSound;
    }

    void OnDisable() {
        playerJumping.OnJumpTriggered -= PlayJumpSound;
        playerWallJumping.OnWallJumpTriggered -= PlayJumpSound;
        playerDeath.OnDeath -= PlayDeathSound;
    }

    // Update is called once per frame
    void PlayJumpSound() {
        AudioClip clip = sounds.FindSound("JumpSound");

        if (clip != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    void PlayDeathSound() {
        AudioClip clip = sounds.FindSound("DeathSound");

        if (clip != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
