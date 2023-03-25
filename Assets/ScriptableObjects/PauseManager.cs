using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PauseManager", menuName = "ScriptableObjects/Managers/PauseManager")]
public class PauseManager : ScriptableObject {
    [SerializeField] PlayerInputManager playerInput;

    void OnEnable() {
        playerInput.OnStartPress += HandleStartPress;
    }

    void OnDisable() {
        playerInput.OnStartPress -= HandleStartPress;
    }

    void HandleStartPress() {
        playerInput.ChangeActionMap(playerInput.Input.Menus);
        SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
    }

    public void Close() {
        SceneManager.UnloadSceneAsync("Pause");
        playerInput.ChangeActionMap(playerInput.Input.Player);
    }
}


