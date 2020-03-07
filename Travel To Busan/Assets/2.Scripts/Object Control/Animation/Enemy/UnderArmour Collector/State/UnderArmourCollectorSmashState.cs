using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderArmourCollectorSmashState : UnderArmourCollectorState
{
    public Transform rangeBox;

    public float damage = 40f;
    public float knockBackPower = 25f;
    public float coolTime = 2f;

    public override string GetStateName()
    {
        return "UnderArmour Collector Smash State";
    }
    public override void Process()
    {
        var hit = Physics2D.BoxCast(rangeBox.position, new Vector2(Mathf.Abs(rangeBox.lossyScale.x), Mathf.Abs(rangeBox.lossyScale.y)), 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            if (!collector.apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.smash))
                collector.apPortrait.Play(UnderArmourCollector._UnderArmourCollectorAnim_.smash);
        }
        else if (!collector.apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.smash))
        {
            Vector2 dir = (collector.Target.transform.position - collector.transform.position);
            collector.LookAt(dir.x < 0f ? Vector2.left : Vector2.right);
            collector.transform.Translate((dir.x < 0 ? Vector2.left : Vector2.right) * collector.info.speed * Time.deltaTime, Space.World);
            if (!collector.apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.run))
                collector.apPortrait.Play(UnderArmourCollector._UnderArmourCollectorAnim_.run);
        }
    }

    #region Animation Event
    public void OnAttack()
    {
        if (collector.CurrState == this)
        {
            var hit = Physics2D.BoxCast(rangeBox.position, new Vector2(Mathf.Abs(rangeBox.lossyScale.x), Mathf.Abs(rangeBox.lossyScale.y)), 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
            hit.collider?.GetComponent<Entity>().BeAttacked(collector, damage, (hit.collider.transform.position - collector.transform.position).normalized, knockBackPower);
        }
    }
    public void OnAttackEnter()
    {
        if (collector.CurrState == this)
        {
            collector.fxAnimator.SetTrigger(UnderArmourCollector._UnderArmourCollectorAnim_.smash);
        }
        else
            collector.fxAnimator.Play("Bridge");
    }
    public void OnAttackExit()
    {
        if (collector.CurrState == this)
        {
            collector.animState.patternBridge.timer = Time.time + coolTime;
            collector.TransitionProcess(collector.animState.patternBridge);
        }
    }
    #endregion
}
