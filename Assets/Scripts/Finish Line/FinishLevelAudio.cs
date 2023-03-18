using System;
using UnityEngine;

public class FinishLevelAudio : MonoBehaviour {
    [SerializeField] FinishLevel finishLevel;
    [SerializeField] AudioManager sounds;
    [SerializeField] AudioSource audioSource;

    public event Action OnSoundFinished;

    void OnEnable() {
        finishLevel.OnLevelFinished += PlayFinishSound;
    }

    void OnDisable() {
        finishLevel.OnLevelFinished -= PlayFinishSound;
    }

    void PlayFinishSound() {
        AudioClip clip = sounds.FindSound("CollectSound"); // this will be changed

        if (clip != null && audioSource) {
            audioSource.clip = clip;
            audioSource.Play();
        }

        Invoke("BroadcastSoundFinished", clip.length);
    }

    void BroadcastSoundFinished() {
        OnSoundFinished?.Invoke();
    }
}
