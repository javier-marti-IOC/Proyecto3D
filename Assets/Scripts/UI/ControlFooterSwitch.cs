using UnityEngine;
using UnityEngine.UI;

public class ControlFooterSwitch : MonoBehaviour
{
    public GameObject keyboardFooter;
    public GameObject ps4Footer;
    public GameObject xboxFooter;
    public GameObject otherGamepadFooter;

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
                ps4Footer.SetActive(false);
                xboxFooter.SetActive(false);
                otherGamepadFooter.SetActive(false);
                keyboardFooter.SetActive(true);
                break;
            case InputDeviceDetector.InputType.PS4:
                keyboardFooter.SetActive(false);
                xboxFooter.SetActive(false);
                otherGamepadFooter.SetActive(false);
                ps4Footer.SetActive(true);
                break;
            case InputDeviceDetector.InputType.Xbox:
                xboxFooter.SetActive(false);
                otherGamepadFooter.SetActive(false);
                ps4Footer.SetActive(false);
                keyboardFooter.SetActive(false);
                break;
            default:
                keyboardFooter.SetActive(false);
                xboxFooter.SetActive(false);
                ps4Footer.SetActive(false);
                otherGamepadFooter.SetActive(true);
                break;
        }
    }
}
