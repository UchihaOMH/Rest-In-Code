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
        if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.trace))
            officeWorker.apPortrait.Play(_OfficeWorkerAnimTrigger_.trace);

        var hit = Physics2D.BoxCast(rangeBox.position, new Vector2(Mathf.Abs(rangeBox.lossyScale.x), Mathf.Abs(rangeBox.lossyScale.y)), 0, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<Player>() == officeWorker.Target)
            {
                officeWorker.TransitionProcess(officeWorker.animationState.attack);
                return;
            }
        }
        else
        {
            //  타깃이 죽었거나 놓침
            if (officeWorker.Target == null || officeWorker.Target.isDead)
            {
                officeWorker.TransitionProcess(officeWorker.animationState.idle);
                return;
            }
            else
            {
                Vector2 dir = officeWorker.Target.transform.position.x - officeWorker.tr.position.x < 0f ? Vector2.left : Vector2.right;
                officeWorker.LookAt(dir);
                officeWorker.tr.Translate(dir * officeWorker.info.speed * Time.deltaTime, Space.World);
            }
        }
    }
}
