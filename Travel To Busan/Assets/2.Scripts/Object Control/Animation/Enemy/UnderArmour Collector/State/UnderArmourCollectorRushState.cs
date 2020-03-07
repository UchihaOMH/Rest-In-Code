using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderArmourCollectorRushState : UnderArmourCollectorState
{
    public Transform rangeBox;

    public float damage = 10f;
    public float knockBackPower = 15f;
    public float coolTime = 2f;

    private Vector2 dir;

    public override string GetStateName()
    {
        return "UnderArmour Collector Rush State";
    }
    public override void Process()
    {
        if (!collector.apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.rush))
        {
            collector.apPortrait.Play(UnderArmourCollector._UnderArmourCollectorAnim_.rush);
            dir = (collector.Target.transform.position - collector.transform.position).normalized;
            collector.LookAt(dir.x < 0f ? Vector2.left : Vector2.right);
        }

        collector.transform.Translate((dir.x < 0f ? Vector2.left : Vector2.right) * collector.info.speed * 2f * Time.deltaTime, Space.World);
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
            collector.fxAnimator.SetTrigger(UnderArmourCollector._UnderArmourCollectorAnim_.rush);
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
            collector.fxAnimator.Play("Bridge");
        }
    }
    #endregion
}
