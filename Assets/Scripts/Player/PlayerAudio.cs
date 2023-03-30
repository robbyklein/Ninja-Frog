using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    // Settings
    [SerializeField] PlayerCollect playerCollect;
    [SerializeField] PlayerJumping playerJumping;
    [SerializeField] PlayerWallJumping playerWallJumping;
    [SerializeField] PlayerDeath playerDeath;
    SoundPlayer soundPlayer;

    void Start() {
        soundPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<SoundPlayer>();
    }

    void OnEnable() {
        playerJumping.OnJump += PlayJumpSound;
        playerWallJumping.OnWallJump += PlayJumpSound;
        playerDeath.OnDeath += PlayDeathSound;
        playerCollect.OnAppleCollected += PlayCollectionSound;
    }

    void OnDisable() {
        playerJumping.OnJump -= PlayJumpSound;
        playerWallJumping.OnWallJump -= PlayJumpSound;
        playerDeath.OnDeath -= PlayDeathSound;
        playerCollect.OnAppleCollected -= PlayCollectionSound;
    }

    void PlayJumpSound() {
        soundPlayer?.PlaySound(SoundEffect.Jump);
    }

    void PlayDeathSound() {
        soundPlayer?.PlaySound(SoundEffect.Death);
    }

    void PlayCollectionSound() {
        soundPlayer?.PlaySound(SoundEffect.Collect);
    }
}
