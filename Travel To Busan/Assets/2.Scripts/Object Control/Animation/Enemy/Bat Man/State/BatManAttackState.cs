using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatManAttackState : BatManState
{
    public Transform rangeBox;
    public float damage = 60f;
    public float coolTime = 2f;
    public float knockBackPower = 50f;

    public override string GetStateName()
    {
        return "Bat Man Attack State";
    }
    public override void Process()
    {
        if (!batMan.apPortrait.IsPlaying(BatMan._BatManAnim_.attack))
        {
            batMan.apPortrait.Play(BatMan._BatManAnim_.attack);
            batMan.LookAt((batMan.Target.transform.position - batMan.transform.position).x < 0f ? Vector2.left : Vector2.right);
        }
    }

    #region Animation Event
    public void OnAttackEnter()
    {
        batMan.fxAnimator.SetTrigger(BatMan._BatManAnim_.attack);
    }
    public void OnAttack()
    {
        var hit = Physics2D.BoxCast(rangeBox.position, new Vector2(Mathf.Abs(rangeBox.lossyScale.x), Mathf.Abs(rangeBox.lossyScale.y)), 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Player>().BeAttacked(batMan, damage, Vector2.zero, 0f, 1f);
            Vector2 dir = hit.transform.position - batMan.transform.position;
            hit.rigidbody.AddForce((dir.x < 0f ? Vector2.left : Vector2.right) * knockBackPower, ForceMode2D.Impulse);
        }
    }
    public void OnAttackExit()
    {
        batMan.animState.patternBridgeState.timer = Time.time + coolTime;
        batMan.TransitionProcess(batMan.animState.patternBridgeState);
    }
    #endregion
}
