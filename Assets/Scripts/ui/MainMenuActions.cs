using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour {
    [SerializeField] MenuControls controls;
    [SerializeField] PlayerInputManager playerInput;

    enum MenuAction {
        Play,
        Options,
        Quit
    }

    void OnEnable() {
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
                SceneManager.LoadScene("Level1");
                playerInput.ChangeActionMap(playerInput.Input.Player);
                break;
            case "Options":
                Debug.Log("Pffff options.");
                break;
        }
    }
}
