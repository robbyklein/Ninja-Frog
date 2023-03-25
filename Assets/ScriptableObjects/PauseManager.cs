using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PauseManager", menuName = "ScriptableObjects/Managers/PauseManager")]
public class PauseManager : ScriptableObject {
    [SerializeField] PlayerInputManager playerInput;

    bool SceneActive = false;

    void OnEnable() {
        playerInput.OnStartPress += HandleStartPress;
        playerInput.OnMenusClosePress += HandleClose;
    }

    void OnDisable() {
        playerInput.OnStartPress -= HandleStartPress;
        playerInput.OnMenusClosePress -= HandleClose;
    }

    void HandleStartPress() {
        if (!SceneActive) {
            Debug.Log("Start pressed! we need to load scene" + playerInput.Input.Player.enabled);
            playerInput.ChangeActionMap(playerInput.Input.Menus);
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
        }

        SceneActive = !SceneActive;

    }


    void HandleClose() {
        Close();
    }

    public void Close() {
        SceneManager.UnloadSceneAsync("Pause");
        playerInput.ChangeActionMap(playerInput.Input.Player);
        SceneActive = !SceneActive;

    }
}


