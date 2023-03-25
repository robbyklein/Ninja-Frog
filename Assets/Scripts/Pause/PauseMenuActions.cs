using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuActions : MonoBehaviour {
    [SerializeField] MenuControls controls;
    [SerializeField] PlayerInputManager playerInput;
    [SerializeField] PauseManager pauseManager;

    void OnEnable() {
        controls.OnMenuItemSelected += HandleSelect;
    }

    void OnDisable() {
        controls.OnMenuItemSelected -= HandleSelect;
    }


    void HandleSelect(string actionName) {
        switch (actionName) {
            case "Resume":
                pauseManager.Close();
                break;
            case "MainMenu":
                SceneManager.LoadScene("MainMenu");
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
