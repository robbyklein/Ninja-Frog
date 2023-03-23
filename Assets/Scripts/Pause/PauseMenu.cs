using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour {
    [SerializeField] PauseManager pauseManager;
    VisualElement root;
    Button resumeButton;
    Button quitButton;

    void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement;
        resumeButton = root.Q<Button>("resume");
        quitButton = root.Q<Button>("quit");
    }

    void OnEnable() {
        resumeButton.clicked += HandleResumeClick;
        quitButton.clicked += HandleQuitClick;
    }

    void OnDisable() {
        resumeButton.clicked -= HandleResumeClick;
        quitButton.clicked -= HandleQuitClick;
    }

    void HandleResumeClick() {
        pauseManager.Close();
    }

    void HandleQuitClick() {
        Application.Quit();
    }
}
