using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Manage store component menu
// Only one can be activa at a time
public class ComponentMenu : MonoBehaviour
{
    public Manager manager;
    private ToggleGroup toggleGroup;
    private Toggle selected = null;
    private Toggle[] toggles;

    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();

        toggles = GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles) {
            toggle.onValueChanged.AddListener(toggleChange);
        }

    }

    // When a component is selected, inform manager and switch to placement mode
    void toggleChange(bool isOn) {
        selected = toggleGroup.ActiveToggles().FirstOrDefault();

        if (selected != null && !manager.isPlacementMode()) {
            manager.activateMode(true, selected.name);
        } else if (selected != null && manager.isPlacementMode()) {
            manager.changeObject(selected.name);
        } else {manager.disableMode();}
    }

    // Deactivate all toggles
    public void deactivateAllToggles() {
        foreach (Toggle toggle in toggles) {
            toggle.isOn = false;
        }
    }

}
