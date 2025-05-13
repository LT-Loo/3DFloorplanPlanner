using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Manage component properties in UI panel
public class ComponentPanel : MonoBehaviour
{
    public TMP_InputField componentName;
    public TMP_InputField posX, posZ, rot;
    public Button delete, close;
    public TextMeshProUGUI errorMsg;
    public Manager manager;
    public Scrollbar rotScroll;

    void Start()
    {
        delete.onClick.AddListener(deleteComponent);
        close.onClick.AddListener(closePanel);

        componentName.onEndEdit.AddListener(value => updateData(componentName, value));
        posX.onEndEdit.AddListener(value => updateData(posX, value));
        posZ.onEndEdit.AddListener(value => updateData(posZ, value));
        rot.onEndEdit.AddListener(value => updateData(rot, value));
        rotScroll.onValueChanged.AddListener(scrollRotateComponent);

        gameObject.SetActive(false);
    }

    // Delete selected component
    public void deleteComponent() {
        manager.deleteComponent();
        closePanel();
    }

    // Close panel if placement mode disabled
    public void closePanel() {
        if (manager.disableMode()) {
            gameObject.SetActive(false);
        }
    }

    // Open panel and load data
    public void openPanel(GameObject selected) {
        componentName.text = selected.name;  
        receiveObjectData(selected);      
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    // Update data from manager
    public void receiveObjectData(GameObject selected) {
        posX.text = selected.transform.position.x.ToString("F2");
        posZ.text = selected.transform.position.z.ToString("F2");

        rot.text = selected.transform.eulerAngles.y.ToString("F2");
        rotScroll.SetValueWithoutNotify(float.Parse(rot.text) / 360f);
        exceedLimit(false);
    }

    // Send new data to manager
    public void updateData(TMP_InputField field, string input) {
        float output;
        if (field == componentName) {
            manager.updateDataFromPanel("name", input);
        }
        if (float.TryParse(input, out output)) {;
            if (field == posX && output <= 4.2f && output >= -4.2f) {
                manager.updateDataFromPanel("posX", input);
            } else if (field == posZ && output <= 4.2f && output >= -4.2f) {
                manager.updateDataFromPanel("posZ", input);
            } else if (field == rot && output > 0.0f && output <= 360.0f) {
                manager.updateDataFromPanel("rot", input);
                rotScroll.SetValueWithoutNotify(output / 360f);
            }
        }
    }

    // Send error message if excess limit
    public void exceedLimit(bool exceed) {
        errorMsg.enabled = exceed;
    }

    // Update rotation value from scrollbar
    void scrollRotateComponent(float value) {
        float deg = value * 360.0f;
        if (deg >= 360f) {deg = 0;}
        manager.updateDataFromPanel("rot", rot.text);
        rot.text = deg.ToString("F2");
    }
}
