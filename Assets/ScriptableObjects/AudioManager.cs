using UnityEngine;

[CreateAssetMenu(fileName = "AudioManager", menuName = "ScriptableObjects/Managers/AudioManager")]
public class AudioManager : ScriptableObject {
    [SerializeField] AudioClip[] sounds;

    public AudioClip FindSound(string name) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == name) {
                return sounds[i];
            }
        }

        return null;
    }
}
