using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 처음한대 맞기 전까진 계속 Idle
/// </summary>
public class UnderArmourCollectorIdleState : UnderArmourCollectorState
{
    public override string GetStateName()
    {
        return "UnderArmour Collector Idle State";
    }
    public override void Process()
    {
        if (!collector.apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.idle))
            collector.apPortrait.Play(UnderArmourCollector._UnderArmourCollectorAnim_.idle);
    }
}
