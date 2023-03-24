using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PauseManager", menuName = "ScriptableObjects/Managers/PauseManager")]
public class PauseManager : ScriptableObject {
    [SerializeField] PlayerInputManager playerInput;

    void OnEnable() {
        playerInput.OnStartPress += HandlePausePress;
        playerInput.OnMenusClosePress += HandleClose;
    }

    void OnDisable() {
        playerInput.OnStartPress -= HandlePausePress;
        playerInput.OnMenusClosePress -= HandleClose;
    }

    void HandlePausePress() {
        SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
        playerInput.ChangeActionMap(playerInput.Input.Menus);
    }


    void HandleClose() {
        Close();
    }

    public void Close() {
        SceneManager.UnloadSceneAsync("Pause");
        if (playerInput) playerInput.ChangeActionMap(playerInput.Input.Player);
    }
}


