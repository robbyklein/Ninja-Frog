using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Pause Manager", menuName = "ScriptableObjects/Managers/PauseManager")]
public class PauseManager : ScriptableObject {
    [SerializeField] PlayerInputManager playerInput;

    bool SceneActive = false;

    void OnEnable() => HandleSubscriptions();
    void OnDisable() => HandleUnSubscriptions();

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

    void HandleSubscriptions() {
        playerInput.OnStartPress += HandlePausePress;
        playerInput.OnMenusMovementChanged += HandleMenuMovement;
        playerInput.OnMenusClosePress += HandleClose;
    }

    void HandleUnSubscriptions() {
        playerInput.OnStartPress -= HandlePausePress;
        playerInput.OnMenusMovementChanged -= HandleMenuMovement;
        playerInput.OnMenusClosePress -= HandleClose;
    }
}


