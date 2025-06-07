using UnityEngine;
using UnityEngine.UI;

public class ControlIconUpdater : MonoBehaviour
{
    public Image iconImage;

    public Sprite keyboardSprite;
    public Sprite ps4Sprite;
    public Sprite xboxSprite;
    public Sprite otherGamepadSprite;

    void OnEnable()
    {
        InputDeviceDetector.OnInputDeviceChanged += UpdateIcon;
        UpdateIcon(InputDeviceDetector.CurrentInputType); // actualiza al inicio
    }

    void OnDisable()
    {
        InputDeviceDetector.OnInputDeviceChanged -= UpdateIcon;
    }

    private void UpdateIcon(InputDeviceDetector.InputType inputType)
    {
        switch (inputType)
        {
            case InputDeviceDetector.InputType.KeyboardMouse:
                iconImage.sprite = keyboardSprite;
                break;
            case InputDeviceDetector.InputType.PS4:
                iconImage.sprite = ps4Sprite;
                break;
            case InputDeviceDetector.InputType.Xbox:
                iconImage.sprite = xboxSprite;
                break;
            default:
                iconImage.sprite = otherGamepadSprite;
                break;
        }
    }
}
