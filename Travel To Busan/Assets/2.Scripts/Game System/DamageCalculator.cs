using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is here for Balance the damage
/// </summary>
public class DamageCalculator : MonoBehaviour
{
    private float minDamagePercent = 0.3f;

    /// <summary>
    /// 방어력이 높으면 피해량 감소. 단, 70%까지만 감소
    /// eg ) 공격력 100인 몬스터가 방어력 100이상인 플레이어를 공격했을 때, 플레이어의 피해량은 30
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="defense"></param>
    /// <returns></returns>
    public float CalcFinalDamage(float damage, float defense)
    {
        float tmp = damage - defense;

        if (tmp <= damage * minDamagePercent)
            return damage * minDamagePercent;
        else
            return (1f - (defense / damage)) * (damage);
    }
}
