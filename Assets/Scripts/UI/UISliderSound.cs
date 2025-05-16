using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISliderSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] private string hoverSoundName = "HoverButton";
    [SerializeField] private string slideSoundName = "Slide";

    private Slider slider;
    private float lastValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        lastValue = slider.value;

        // Escucha el cambio de valor para cualquier entrada
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    private void OnSliderValueChanged(float newValue)
    {
        if (Mathf.Abs(newValue - lastValue) > Mathf.Epsilon)
        {
            PlaySlideSound();
            lastValue = newValue;
        }
    }

    private void PlayHoverSound()
    {
        if (!string.IsNullOrEmpty(hoverSoundName))
        {
            AudioManager.Instance.Play(hoverSoundName);
        }
    }

    private void PlaySlideSound()
    {
        if (!string.IsNullOrEmpty(slideSoundName))
        {
            AudioManager.Instance.Play(slideSoundName);
        }
    }
}
