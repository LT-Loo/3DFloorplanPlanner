using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    void Update()
    {
    }

    public void deleteComponent() {
        manager.deleteComponent();
        closePanel();
    }

    public void closePanel() {
        if (manager.disableMode()) {
            gameObject.SetActive(false);
        }
    }

    public void openPanel(GameObject selected) {
        componentName.text = selected.name;  
        receiveObjectData(selected);      
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    public void receiveObjectData(GameObject selected) {
        posX.text = selected.transform.position.x.ToString("F2");
        posZ.text = selected.transform.position.z.ToString("F2");

        rot.text = selected.transform.rotation.x.ToString("F2");
        rotScroll.value = float.Parse(rot.text) / 360f;
        exceedLimit(false);
    }

    public void updateData(TMP_InputField field, string input) {
        float output;
        if (field == componentName) {
            manager.updateDataFromPanel("name", input);
        }
        if (float.TryParse(input, out output)) {;
            Debug.Log("updating");
            if (field == posX) {
                manager.updateDataFromPanel("posX", input);
            } else if (field == posZ) {
                manager.updateDataFromPanel("posZ", input);
            } else if (field == rot && output >= 0.0f && output <= 360.0f) {
                rotScroll.value = float.Parse(rot.text) / 360f;
                manager.updateDataFromPanel("rot", input);
            }
        }
    }

    public void exceedLimit(bool exceed) {
        errorMsg.enabled = exceed;
    }

    void scrollRotateComponent(float value) {
        rot.text = (value * 360.0f).ToString("F2");
        manager.updateDataFromPanel("rot", rot.text);
    }
}
