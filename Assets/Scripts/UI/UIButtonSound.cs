using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [SerializeField] private string hoverSoundName = "HoverButton";
    [SerializeField] private string clickSoundName = "ClickButton";

    private bool allowSelectionSound = false;

    private void Start()
    {
        // Evita sonido inmediato al abrir menú: lo activa tras pequeño retraso
        Invoke(nameof(EnableSelectionSound), 0.1f);
    }

    private void EnableSelectionSound()
    {
        allowSelectionSound = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayClickSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (allowSelectionSound)
        {
            PlayHoverSound();
        }
    }

    private void PlayHoverSound()
    {
        if (!string.IsNullOrEmpty(hoverSoundName))
        {
            AudioManager.Instance.Play(hoverSoundName);
        }
    }

    private void PlayClickSound()
    {
        if (!string.IsNullOrEmpty(clickSoundName))
        {
            AudioManager.Instance.Play(clickSoundName);
        }
    }
}
