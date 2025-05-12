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
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();

        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles) {
            toggle.onValueChanged.AddListener(toggleChange);
        }

    }

    void Update()
    {
        
    }

    void toggleChange(bool isOn) {
        selected = toggleGroup.ActiveToggles().FirstOrDefault();
        // Debug.Log(selected.name);

        if (selected != null) {
            manager.activateMode(false, selected.name);
        } else {manager.disableMode();}
    }
}
