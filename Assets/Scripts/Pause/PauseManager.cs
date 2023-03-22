using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    [SerializeField] SceneAsset pauseScene;
    [SerializeField] PlayerInput playerInput;

    bool SceneActive = false;

    void OnEnable() {
        playerInput.OnStartPress += HandlePausePress;
        playerInput.OnMenusMovementChanged += HandleMenuMovement;
        playerInput.OnMenusClosePress += HandleClose;
    }

    void OnDisable() {
        playerInput.OnStartPress -= HandlePausePress;
        playerInput.OnMenusMovementChanged -= HandleMenuMovement;
        playerInput.OnMenusClosePress -= HandleClose;
    }

    void HandlePausePress() {
        if (!SceneActive) {
            SceneManager.LoadScene(pauseScene.name, LoadSceneMode.Additive);
            playerInput.ChangeActionMap(playerInput.Input.Menus);
        }

        SceneActive = !SceneActive;
    }

    void HandleMenuMovement(Vector2 movement) {
        Debug.Log("This is triggered");
    }

    void HandleClose() {
        playerInput.ChangeActionMap(playerInput.Input.Player);
        SceneManager.UnloadSceneAsync(pauseScene.name);
        SceneActive = !SceneActive;
    }
}


