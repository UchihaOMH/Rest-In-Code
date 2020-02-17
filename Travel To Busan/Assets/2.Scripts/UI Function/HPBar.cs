using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] private RectTransform hpBar;

    public bool IsInvisible
    {
        get => isInvisible;
        private set => isInvisible = value;
    }
    private bool isInvisible;

    private void Awake()
    {
        hpBar = GetComponent<RectTransform>();
    }

    public void FillAmount(float _amount)
    {
        float amount;

        if (_amount <= 0f)
            amount = 0f;
        else
            amount = _amount;

        hpBar.anchorMax = new Vector2(amount, hpBar.anchorMax.y);
    }

    public void HideBar(bool hide)
    {
        IsInvisible = hide;

        if (hide)
        {
            GetComponentInParent<CanvasGroup>().alpha = 0f;
        }
        else
        {
            GetComponentInParent<CanvasGroup>().alpha = 1f;
        }
    }
}