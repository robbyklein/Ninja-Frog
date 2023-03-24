using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour {
    [SerializeField] PauseManager pauseManager;
    [SerializeField] PlayerInputManager playerInput;

    VisualElement root;
    Button resumeButton;
    Button quitButton;
    Button menuButton;

    int activeMenuItemIndex = 0;

    void OnEnable() {
        root ??= GetComponent<UIDocument>().rootVisualElement;
        menuButton ??= root.Q<Button>("main-menu");
        resumeButton ??= root.Q<Button>("resume");
        quitButton ??= root.Q<Button>("quit");

        resumeButton.clicked += HandleResumeClick;
        quitButton.clicked += HandleQuitClick;

        playerInput.OnMenusMovementChanged += HandleMovementChange;
        playerInput.OnMenusSelectPress += HandleSelectPress;
        playerInput.OnMenusClosePress += HandleClosePress;
    }

    void OnDisable() {
        resumeButton.clicked -= HandleResumeClick;
        quitButton.clicked -= HandleQuitClick;
        playerInput.OnMenusMovementChanged -= HandleMovementChange;
        playerInput.OnMenusSelectPress -= HandleSelectPress;
        playerInput.OnMenusClosePress -= HandleClosePress;
    }

    void HandleResumeClick() {
        pauseManager.Close();
    }

    void HandleQuitClick() {
        Application.Quit();
    }

    void HandleClosePress() {
        HandleResumeClick();
    }

    void HandleSelectPress() {
        if (activeMenuItemIndex == 0) {
            HandleResumeClick();
        } else if (activeMenuItemIndex == 1) {
        } else if (activeMenuItemIndex == 2) {
            HandleQuitClick();
        }
    }

    void HandleMovementChange(Vector2 movement) {
        // Up down
        if (movement.y > 0 && activeMenuItemIndex > 0f) {
            activeMenuItemIndex--;
            UpdateButtonColors();
        } else if (movement.y < 0f && activeMenuItemIndex < 2) {
            activeMenuItemIndex++;
            UpdateButtonColors();
        }
    }

    void UpdateButtonColors() {
        // Remove active from all
        menuButton.RemoveFromClassList("menu-item-active");
        resumeButton.RemoveFromClassList("menu-item-active");
        quitButton.RemoveFromClassList("menu-item-active");

        Debug.Log("Removed from all");

        // add new active
        if (activeMenuItemIndex == 0) {
            resumeButton.AddToClassList("menu-item-active");
            Debug.Log("highlight resume");

        } else if (activeMenuItemIndex == 1) {
            menuButton.AddToClassList("menu-item-active");
            Debug.Log("highlight resume");

        } else if (activeMenuItemIndex == 2) {
            quitButton.AddToClassList("menu-item-active");
        }


    }
}
