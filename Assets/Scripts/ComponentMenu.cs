using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    void Update()
    {
        
    }

    void toggleChange(bool isOn) {
        selected = toggleGroup.ActiveToggles().FirstOrDefault();

        if (selected != null) {
            manager.activateMode(true, selected.name);
        } else {manager.disableMode();}
    }

    public void deactivateAllToggles() {
        foreach (Toggle toggle in toggles) {
            toggle.isOn = false;
        }
    }

}
