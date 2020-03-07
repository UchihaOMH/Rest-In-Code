using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public CanvasGroup theEndCg;
    public CanvasGroup quitGameCg;

    public void ShowEnding()
    {
        StartCoroutine(FadeOut());
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (theEndCg.alpha < 1f)
        {
            timer += Time.deltaTime / 2f;
            theEndCg.alpha = Mathf.Lerp(0f, 1f, timer);
            yield return null;
        }

        StartCoroutine(QuitGameButtonFadeIn());
    }
    private IEnumerator QuitGameButtonFadeIn()
    {
        float timer = 0f;
        while (quitGameCg.alpha < 1f)
        {
            timer += Time.deltaTime / 1.5f;
            quitGameCg.alpha = Mathf.Lerp(0f, 1f, timer);
            yield return null;
        }
    }
}
