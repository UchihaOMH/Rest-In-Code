using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderArmourCollectorPatternBridgeState : UnderArmourCollectorState
{
    [Header("Pattern Weight"), Space(15f)]
    public float attackPatternWeight = 0.45f;
    public float smashPatternWeight = 0.35f;
    public float rushPatternWeight = 0.2f;
    public float throwAttackPatternWeight = 0.3f;

    public float timer = 0f;

    public override string GetStateName()
    {
        return "UnderArmour Collector Run State";
    }
    public override void Process()
    {
        if (Time.time >= timer)
        {
            float random = Random.Range(0f, attackPatternWeight + smashPatternWeight + rushPatternWeight + throwAttackPatternWeight);
            if (random <= attackPatternWeight)
            {
                collector.TransitionProcess(collector.animState.attack);
            }
            else if (random <= attackPatternWeight + smashPatternWeight)
            {
                collector.TransitionProcess(collector.animState.smash);
            }
            else if (random <= attackPatternWeight + smashPatternWeight + rushPatternWeight)
            {
                collector.TransitionProcess(collector.animState.rush);
            }
            else if (random <= attackPatternWeight + smashPatternWeight + rushPatternWeight + throwAttackPatternWeight)
            {
                collector.TransitionProcess(collector.animState.throwAttack);
            }
        }
        else
        {
            collector.LookAt((collector.Target.transform.position - collector.transform.position).x < 0f ? Vector2.left : Vector2.right);
            collector.apPortrait.Play(UnderArmourCollector._UnderArmourCollectorAnim_.idle);
        }
    }
}
