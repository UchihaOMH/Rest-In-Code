using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환 및 Fade기능 포함
/// </summary>
public class Fade : MonoBehaviour
{
    #region Public Field
    public Sprite fadeSprite;
    public float duration = 1.0f;
    #endregion

    #region Private Field
    private CanvasGroup cg;

    private bool isProceeding = false;
    private Action callback;
    #endregion

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public void LoadScene(string scene, Action _callback = null)
    {
        if (!isProceeding)
        {
            callback = _callback;
            StartCoroutine(LoadSceneCoroutine(scene));
        }
    }
    public void LoadScene(int sceneIdx, Action _callback = null)
    {
        if (!isProceeding)
        {
            callback = _callback;
            StartCoroutine(LoadSceneCoroutine(sceneIdx));
        }
    }

    private IEnumerator LoadSceneCoroutine(string scene)
    {
        isProceeding = true;
        float timer = 0.0f;

        //  Fade out
        while (cg.alpha < 1.0f)
        {
            timer += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(0.0f, 1.0f, timer);

            yield return null;
        }
        timer = 0.0f;

        //  Load scene
        callback?.Invoke();
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

        //  Fade in
        while (cg.alpha > 0.0f)
        {
            timer += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(1.0f, 0.0f, timer);

            yield return null;
        }
        isProceeding = false;
    }
    private IEnumerator LoadSceneCoroutine(int sceneIdx)
    {
        isProceeding = true;
        float timer = 0.0f;

        //  Fade out
        while (cg.alpha < 1.0f)
        {
            timer += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(0.0f, 1.0f, timer);

            yield return null;
        }
        timer = 0.0f;

        //  Load scene
        callback?.Invoke();
        SceneManager.LoadScene(sceneIdx, LoadSceneMode.Single);

        //  Fade in
        while (cg.alpha > 0.0f)
        {
            timer += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(1.0f, 0.0f, timer);

            yield return null;
        }
        isProceeding = false;
    }
}
