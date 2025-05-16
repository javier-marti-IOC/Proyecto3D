using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class UITMPDropdownSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] private string hoverSoundName = "HoverButton";
    [SerializeField] private string selectSoundName = "ClickButton";

    private TMP_Dropdown dropdown;
    private int lastValue;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        lastValue = dropdown.value;

        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    private void OnValueChanged(int newValue)
    {
        if (newValue != lastValue)
        {
            PlaySelectSound();
            lastValue = newValue;
        }
    }

    private void PlayHoverSound()
    {
        if (!string.IsNullOrEmpty(hoverSoundName))
            AudioManager.Instance.Play(hoverSoundName);
    }

    private void PlaySelectSound()
    {
        if (!string.IsNullOrEmpty(selectSoundName))
            AudioManager.Instance.Play(selectSoundName);
    }
}
