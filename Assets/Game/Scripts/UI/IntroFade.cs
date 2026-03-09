using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroFade : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI subtitle;
    public Button startButton;

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        yield return FadeText(title, 2f);

        yield return new WaitForSeconds(1f);

        yield return FadeText(subtitle, 2f);

        yield return new WaitForSeconds(1f);

        yield return FadeButton(startButton, 2f);
    }

    IEnumerator FadeText(TextMeshProUGUI text, float duration)
    {
        float time = 0;
        Color color = text.color;

        while (time < duration)
        {
            color.a = Mathf.Lerp(0, 1, time / duration);
            text.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        text.color = color;
    }

    IEnumerator FadeButton(Button button, float duration)
    {
        Image img = button.GetComponent<Image>();
        TextMeshProUGUI txt = button.GetComponentInChildren<TextMeshProUGUI>();

        float time = 0;
        Color imgColor = img.color;
        Color txtColor = txt.color;

        while (time < duration)
        {
            float a = Mathf.Lerp(0, 1, time / duration);

            imgColor.a = a;
            txtColor.a = a;

            img.color = imgColor;
            txt.color = txtColor;

            time += Time.deltaTime;
            yield return null;
        }
    }
}