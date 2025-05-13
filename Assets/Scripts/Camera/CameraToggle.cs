using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

// Visual change for pan and rotate toggles
public class ToggleController : MonoBehaviour
{
    public Toggle toggle;
    public Image background;
    public Color onColor = new Color(124/255f, 137/255f, 173/255f);
    public Color offColor = new Color(207/255f, 207/255f, 207/255f);


    void Start()
    {
        toggle.onValueChanged.AddListener(UpdateColor);
        UpdateColor(toggle.isOn);
    }

    // Toggle on - Blue, off - grey
    void UpdateColor(bool isOn) {
        if (background != null) {
            background.color = isOn ? onColor : offColor;
        }
    }

}
