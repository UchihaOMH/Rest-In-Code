using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerTraceState : OfficeWorkerState, IAnimState
{
    private Entity target;

    public string GetStateName()
    {
        return "Run State";
    }
    public void Process()
    {
        if (target == null || target.isDead)
        {
            officeWorker.TransitionProcess(officeWorker.animationState.patrol);
            officeWorker.anim.SetBool(_OfficeWorkerAnimTrigger_.bChase, false);
            target = null;
            return;
        }
        else
        {
            officeWorker.anim.SetBool(_OfficeWorkerAnimTrigger_.bChase, true);
            Vector2 dir = new Vector2(target.transform.position.x - officeWorker.tr.position.x, target.transform.position.y - officeWorker.tr.position.y);
            officeWorker.tr.Translate(dir.normalized * officeWorker.info.speed, Space.World);
        }
    }
    public void SetTarget(Entity _target)
    {
        target = _target;
    }
}
