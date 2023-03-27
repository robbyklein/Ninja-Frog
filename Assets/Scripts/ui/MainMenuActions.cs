using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour {
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
            case "Quit":
                Application.Quit();
                break;
            case "Play":
                SceneManager.LoadScene("Levels");
                break;
            case "Options":
                Debug.Log("Pffff options.");
                break;
        }
    }
}
