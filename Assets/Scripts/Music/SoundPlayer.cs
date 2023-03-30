using UnityEngine;

public enum SoundEffect {
    Jump,
    Collect,
    Death,
    MenuChange,
    MenuSelect
}

public class SoundPlayer : MonoBehaviour {
    private static SoundPlayer instance = null;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioManager audioManager;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySound(SoundEffect soundEffect) {
        Debug.Log("Play sound triggered!");

        switch (soundEffect) {
            case SoundEffect.Jump:
                audioSource.PlayOneShot(audioManager.FindSound("JumpSound"));
                break;
            case SoundEffect.Collect:
                audioSource.PlayOneShot(audioManager.FindSound("CollectSound"));
                break;
            case SoundEffect.Death:
                audioSource.PlayOneShot(audioManager.FindSound("DeathSound"));
                break;
            case SoundEffect.MenuChange:
                audioSource.PlayOneShot(audioManager.FindSound("menu-change"));
                break;
            case SoundEffect.MenuSelect:
                audioSource.PlayOneShot(audioManager.FindSound("menu-select"));
                break;
        }
    }
}