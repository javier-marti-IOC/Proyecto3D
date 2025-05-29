using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputDeviceDetector : MonoBehaviour
{
    public enum InputType { KeyboardMouse, PS4, Xbox, Other }

    public static InputType CurrentInputType { get; private set; }

    public delegate void InputDeviceChanged(InputType newInputType);
    public static event InputDeviceChanged OnInputDeviceChanged;

    private PlayerInput playerInput;
    private InputType lastInputType;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        DetectCurrentControlScheme(playerInput.currentControlScheme);
    }

    void OnEnable()
    {
        playerInput.onControlsChanged += OnControlsChanged;
    }

    void OnDisable()
    {
        playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        DetectCurrentControlScheme(input.currentControlScheme);
    }

    private void DetectCurrentControlScheme(string scheme)
    {
        InputType newType = InputType.Other;

        if (scheme == "KeyboardMouse")
            newType = InputType.KeyboardMouse;
        else if (scheme == "Gamepad")
        {
            var gamepad = Gamepad.current;
            if (gamepad != null)
            {
                string name = gamepad.name.ToLower();

                if (name.Contains("dualshock") || name.Contains("ps"))
                    newType = InputType.PS4;
                else if (name.Contains("xbox"))
                    newType = InputType.Xbox;
                else
                    newType = InputType.Other;
            }
        }

        if (newType != lastInputType)
        {
            lastInputType = newType;
            CurrentInputType = newType;
            OnInputDeviceChanged?.Invoke(newType);
        }
    }
}
