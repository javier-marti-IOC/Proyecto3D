using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
    //public CanvasGroup canvGroup;
    public RectTransform panel;
    public TextMeshProUGUI panelText;

    //private bool mFaded = false;
    public float duration;

    public void ShowPanel(string phrase)
    {
        gameObject.SetActive(true);
        panelText.text = phrase;
    }
    public void HidePanel()
    {
        gameObject.SetActive(false);
        panelText.text = "";
    }

    /* public void Fade()
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        mFaded = !mFaded;
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, mFaded ? 1 : 0));
    }

    public IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while(counter < duration)
        {
            counter += Time.deltaTime;
            //canvGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    } */
}
