using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerTraceState : OfficeWorkerState, IAnimState
{
    /// <summary>
    //  접근해야하는 최소한의 거리. 접근에 성공하면 공격한다
    /// </summary>
    public Transform rangeBox;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(rangeBox.position, rangeBox.lossyScale);
    }

    public string GetStateName()
    {
        return "Trace State";
    }
    public void Process()
    {
        var hit = Physics2D.BoxCast(rangeBox.position, new Vector2(Mathf.Abs(rangeBox.lossyScale.x), Mathf.Abs(rangeBox.lossyScale.y)), 0, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<Player>() == officeWorker.target)
            {
                officeWorker.TransitionProcess(officeWorker.animationState.attack);
                (officeWorker.CurrState as OfficeWorkerAttackState).Attack();
            }
        }
        else
        {
            //  타깃이 죽었거나 놓침
            if (officeWorker.target == null || officeWorker.target.isDead)
            {
                officeWorker.TransitionProcess(officeWorker.animationState.patrol);
            }
            else
            {
                if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.trace))
                    officeWorker.apPortrait.CrossFade(_OfficeWorkerAnimTrigger_.trace);
                Vector2 dir = officeWorker.target.transform.position.x - officeWorker.tr.position.x < 0f ? Vector2.left : Vector2.right;
                officeWorker.LookAt(dir);
                officeWorker.tr.Translate(dir * officeWorker.info.speed * Time.deltaTime, Space.World);
            }
        }
    }
}
