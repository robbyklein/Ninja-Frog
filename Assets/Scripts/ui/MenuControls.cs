using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuControls : MonoBehaviour {
    // Player input
    [SerializeField] PlayerInputManager playerInput;
    SoundPlayer soundPlayer;

    // Ui elements
    VisualElement root;
    List<Button> Options;

    // Menu State
    int ActiveOptionIndex = 0;

    // Events
    public event Action<string> OnMenuItemSelected;

    void Start() {
        soundPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<SoundPlayer>();
    }

    void OnEnable() {
        // Get root el
        root ??= GetComponent<UIDocument>().rootVisualElement;

        // Count menu items
        Options = root.Query<Button>(className: "menu-item").ToList();

        // Subscribe to controls
        playerInput.OnMenusMovementChanged += HandleMovement;
        playerInput.OnMenusSelectPress += HandleSelect;

        // Subscribe to clicks on each item
        for (int i = 0; i < Options.Count; i++) {
            int index = i; // no clue why this is needed
            Button option = Options[i];
            option.RegisterCallback<MouseDownEvent>(e => HandleClick(option), TrickleDown.TrickleDown);
            option.RegisterCallback<PointerEnterEvent>(e => HandleHover(index));
        }
    }

    void OnDisable() {
        // Unsubscribe to controls
        playerInput.OnMenusMovementChanged -= HandleMovement;
        playerInput.OnMenusSelectPress -= HandleSelect;

        // Unsubscribe to clicks on each item
        for (int i = 0; i < Options.Count; i++) {
            int index = i;
            Button option = Options[i];
            option.UnregisterCallback<MouseDownEvent>(e => HandleClick(option), TrickleDown.TrickleDown);
            option.UnregisterCallback<PointerEnterEvent>(e => HandleHover(index));
        }
    }

    void HighlightButton(int oldIndex, int newIndex) {
        Options[oldIndex].RemoveFromClassList("menu-item-active");
        Options[newIndex].AddToClassList("menu-item-active");
    }

    void HandleMovement(Vector2 movement) {
        if (movement.y == 0f) return;

        int oldActiveIndex = ActiveOptionIndex;

        if (movement.y > 0f && ActiveOptionIndex > 0) {
            ActiveOptionIndex--;
            PlayChangeSound();
        } else if (movement.y < 0f && ActiveOptionIndex < Options.Count - 1) {
            ActiveOptionIndex++;
            PlayChangeSound();
        } else {
            return;
        }

        HighlightButton(oldActiveIndex, ActiveOptionIndex);
    }

    void PlayChangeSound() {
        soundPlayer?.PlaySound(SoundEffect.MenuChange);
    }

    void PlaySelectSound() {
        soundPlayer?.PlaySound(SoundEffect.MenuSelect);
    }

    void HandleSelect() {
        string name = Options[ActiveOptionIndex].name;
        OnMenuItemSelected?.Invoke(name);
        PlaySelectSound();
    }

    void HandleClick(Button option) {
        OnMenuItemSelected?.Invoke(option.name);
        PlaySelectSound();
    }

    void HandleHover(int index) {
        HighlightButton(ActiveOptionIndex, index);
        ActiveOptionIndex = index;
        PlayChangeSound();
    }
}
