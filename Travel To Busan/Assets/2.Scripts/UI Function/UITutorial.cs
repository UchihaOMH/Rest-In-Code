using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    public GameObject canvas;
    public Image image;
    public List<Sprite> tutorial;

    private int idx = 0;

    private void Awake()
    {
        idx = 0;
        image.sprite = tutorial[idx];

        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (idx == tutorial.Count)
        {
            canvas.SetActive(false);
            return;
        }

        try
        {
#if UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                image.sprite = tutorial[++idx];
#endif

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Mouse0))
                image.sprite = tutorial[++idx];
#endif
        }
        catch (System.Exception)
        {

        }
    }
}
