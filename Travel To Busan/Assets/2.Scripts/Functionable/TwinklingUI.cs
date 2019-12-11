using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI에 캔버스 그룹 컴포넌트를 추가하여, 깜빡임 기능을 구현
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class TwinklingUI : MonoBehaviour
{
    public float twinkleSpeed = 1f;
    public bool BTwinkle
    {
        get => bTwinkle; 
        set
        {
            if (!bTwinkle && value)
            {
                bTwinkle = value;
                StartCoroutine(TwinkleCoroutine());
            }
            else
                bTwinkle = value;
        }
    }

    private CanvasGroup cg;
    [SerializeField]
    private bool bTwinkle = false;

    #region Mono
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();

        BTwinkle = true;
    }
    #endregion

    #region Coroutine
    public IEnumerator TwinkleCoroutine()
    {
        cg.alpha = 1f;

        while (BTwinkle)
        {
            while (cg.alpha > 0.0f)
            {
                cg.alpha -= Mathf.Lerp(0.0f, 1f, Time.deltaTime * twinkleSpeed);

                yield return null;
            }

            while (cg.alpha < 1.0f)
            {
                cg.alpha += Mathf.Lerp(0.0f, 1f, Time.deltaTime * twinkleSpeed);

                yield return null;
            }
        }

        cg.alpha = 1f;
        yield return null;
    }
    #endregion
}
