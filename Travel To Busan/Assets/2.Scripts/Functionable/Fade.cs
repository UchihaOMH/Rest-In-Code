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
    public static Fade Instance { get => instance; private set => instance = value; }

    #region Public Field
    public Sprite fadeSprite;
    public float duration = 1.0f;
    #endregion

    #region Private Field
    private static Fade instance;
    private Action callback;
    private GameObject fadeObj;
    private GameObject fadeCanvas;

    private bool isProceeding = false;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        //  Create Fade Canvask object and init it
        fadeCanvas = new GameObject("Fade Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        fadeCanvas.transform.SetParent(this.gameObject.transform);
        fadeCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        //  Create Fade Panel game object and init it
        fadeObj = new GameObject("Fade Panel", typeof(Image), typeof(CanvasGroup));
        fadeObj.transform.SetParent(fadeCanvas.transform);
        fadeObj.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        fadeObj.GetComponent<RectTransform>().anchorMax = Vector2.one;
        fadeObj.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        fadeObj.GetComponent<RectTransform>().anchoredPosition = fadeCanvas.GetComponent<RectTransform>().anchoredPosition;

        //Attach Sprite
        fadeObj.GetComponent<Image>().sprite = fadeSprite;
        fadeObj.GetComponent<Image>().color = Color.black;

        //Set Panel Transparency
        fadeObj.GetComponent<CanvasGroup>().alpha = 0.0f;
        fadeObj.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void LoadScene(string scene, Action callback = null, Sprite fadeSprite = null)
    {
        this.callback = callback;

        if (fadeSprite != null)
            fadeObj.GetComponent<Image>().sprite = fadeSprite;

        if (!isProceeding)
            StartCoroutine(LoadSceneCoroutine(scene));
    }

    private IEnumerator LoadSceneCoroutine(string scene)
    {
        isProceeding = true;
        float timer = 0.0f;
        CanvasGroup cg = fadeObj.GetComponent<CanvasGroup>();

        //  Fade out
        while (cg.alpha < 1.0f)
        {
            timer += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(0.0f, 1.0f, timer);

            yield return null;
        }
        cg.alpha = 1.0f;
        timer = 0.0f;

        //  Load scene
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

        //  Fade in
        while (cg.alpha > 0.0f)
        {
            timer += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(1.0f, 0.0f, timer);

            yield return null;
        }
        cg.alpha = 0.0f;
        isProceeding = false;
    }
}
