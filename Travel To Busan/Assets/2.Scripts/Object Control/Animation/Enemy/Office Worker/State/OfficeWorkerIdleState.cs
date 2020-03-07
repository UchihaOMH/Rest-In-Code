using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerIdleState : OfficeWorkerState, IAnimState
{
    public Transform detectRange;

    public string GetStateName()
    {
        return "Office Worker Idle State";
    }
    public void Process()
    {
        if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.idle))
            officeWorker.apPortrait.Play(_OfficeWorkerAnimTrigger_.idle);

        var hit = Physics2D.CircleCast(detectRange.position, Mathf.Abs(detectRange.lossyScale.x / 2f), Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider?.GetComponent<Player>() != null && hit.collider?.GetComponent<Player>().isDead == false)
        {
            officeWorker.FocusTarget(hit.collider.gameObject.GetComponent<Player>());
            officeWorker.TransitionProcess(officeWorker.animationState.trace);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectRange.position, detectRange.lossyScale.x / 2f);
    }
}
