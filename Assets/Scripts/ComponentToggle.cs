using UnityEngine;
using UnityEngine.UI;

public class ComponentToggle : MonoBehaviour
{
    public Toggle toggle;
    public Image background;
    public Color onColor = new Color(106/255f, 227/255f, 158/255f);
    public Color offColor = new Color(1.0f, 1.0f, 1.0f);

    void Start()
    {
        toggle.onValueChanged.AddListener(UpdateColor);
        UpdateColor(toggle.isOn);
    }

    void Update()
    {
        
    }

    void UpdateColor(bool isOn) {
        if (background != null) {
            background.color = isOn ? onColor : offColor;
        }
    }
}
