using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
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
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
            playerInput.ChangeActionMap(playerInput.Input.Menus);
        }

        SceneActive = !SceneActive;
    }

    void HandleMenuMovement(Vector2 movement) {
        Debug.Log("This is triggered");
    }

    void HandleClose() {
        Close();
    }

    public void Close() {
        if (playerInput) playerInput.ChangeActionMap(playerInput.Input.Player);
        SceneManager.UnloadSceneAsync("Pause");
        SceneActive = !SceneActive;
    }
}


