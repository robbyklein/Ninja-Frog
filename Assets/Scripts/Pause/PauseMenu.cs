using UnityEngine;

public class PauseMenu : MonoBehaviour {
    [SerializeField] PauseManager pauseManager;
    [SerializeField] PlayerInputManager playerInput;


    void OnEnable() {
        playerInput.OnMenusClosePress += HandleClosePress;
    }

    void OnDisable() {
        playerInput.OnMenusClosePress -= HandleClosePress;
    }



    void HandleClosePress() {
        pauseManager.Close();
    }
}
