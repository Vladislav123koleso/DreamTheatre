using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image fadeImage; // Image для фейд-эффекта
    public float fadeDuration = 1.0f; // Длительность эффекта фейда

    private void Awake()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(true); 
        }
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        yield return StartCoroutine(Fade(1));
    }

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(0));
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float timeElapsed = 0;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        // Завершаем фейд, выставляя точное значение альфа
        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;
    }
}
