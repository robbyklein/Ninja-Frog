using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudio", menuName = "ScriptableObjects/Manager/PlayerAudio")]
public class AudioManager : ScriptableObject
{
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip collectSound;

    public AudioClip JumpSound
    {
        get { return jumpSound; }
    }

    public AudioClip DeathSound
    {
        get
        {
            return deathSound;
        }
    }

    public AudioClip CollectSound
    {
        get { return collectSound; }
    }
}
