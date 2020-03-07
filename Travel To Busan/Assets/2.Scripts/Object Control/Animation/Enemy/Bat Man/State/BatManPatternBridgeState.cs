using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatManPatternBridgeState : BatManState
{
    public Transform closeAttackRangeBox;

    [Header("Pattern Weight"), Space(15f)]
    public float attackPatternWeight = 0.3f;
    public float swingPatternWeight = 0.3f;

    public float timer = 0f;

    public override string GetStateName()
    {
        return "Bat Man Pattern Bridge State";
    }
    public override void Process()
    {
        if (Time.time >= timer)
        {
            var hit = Physics2D.BoxCast(closeAttackRangeBox.position, new Vector2(Mathf.Abs(closeAttackRangeBox.lossyScale.x), Mathf.Abs(closeAttackRangeBox.lossyScale.y)), 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
            if (hit.collider != null)
            {
                float random = Random.Range(0f, attackPatternWeight + swingPatternWeight);

                if (random <= attackPatternWeight)
                    batMan.TransitionProcess(batMan.animState.attackState);
                else if (random <= attackPatternWeight + swingPatternWeight)
                    batMan.TransitionProcess(batMan.animState.swingState);
            }
            else
            {
                batMan.TransitionProcess(batMan.animState.shoutBallState);
            }
        }
        else
            if (!batMan.apPortrait.IsPlaying(BatMan._BatManAnim_.idle))
            batMan.apPortrait.Play(BatMan._BatManAnim_.idle);
    }
}
