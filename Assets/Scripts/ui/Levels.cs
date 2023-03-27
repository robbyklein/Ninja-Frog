using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour {
    [SerializeField] MenuControls controls;
    [SerializeField] PlayerInputManager playerInput;

    void OnEnable() {
        playerInput.ChangeActionMap(playerInput.Input.Menus);
        controls.OnMenuItemSelected += HandleSelect;
    }

    void OnDisable() {
        controls.OnMenuItemSelected -= HandleSelect;
    }


    void HandleSelect(string actionName) {
        switch (actionName) {
            case "MainMenu":
                SceneManager.LoadScene("MainMenu");
                break;
            default:
                playerInput.ChangeActionMap(playerInput.Input.Player);
                SceneManager.LoadScene(actionName);
                break;
        }
    }
}
