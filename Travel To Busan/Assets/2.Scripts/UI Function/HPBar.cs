using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] private RectTransform hpBar;

    public void FillAmount(float _amount)
    {
        float amount;

        if (_amount <= 0f)
            amount = 0f;
        else
            amount = _amount;

        hpBar.anchorMax = new Vector2(amount, hpBar.anchorMax.y);
    }
}