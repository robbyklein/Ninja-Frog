using UnityEngine;

[CreateAssetMenu(fileName = "Collectable", menuName = "ScriptableObjects/Item/Collectable")]
public class Collectable : ScriptableObject {
    [SerializeField] AudioClip track;
}
