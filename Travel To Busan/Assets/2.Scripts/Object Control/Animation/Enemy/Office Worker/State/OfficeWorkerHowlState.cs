using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerHowlState : OfficeWorkerState, IAnimState
{
    public float howlRadius = 3f;

    private Entity target;

    public string GetStateName()
    {
        return "Howl State";
    }
    private void Awake()
    {
        officeWorker.anim.GetBehaviour<OfficeWorkerHowlBehaviour>().AddExitEvent(() =>
        {
            officeWorker.FocusTarget(target);
        });
    }
    public void Process()
    {
        
    }
    public void Howl(Entity _target)
    {
        target = _target;
        officeWorker.anim.SetTrigger(_OfficeWorkerAnimTrigger_.tHowl);

        var hits = Physics2D.CircleCastAll(officeWorker.tr.position, howlRadius, Vector2.up, 0f, LayerMask.GetMask(GameConst.LayerDefinition.enemy));
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].collider.gameObject.Equals(officeWorker.gameObject))
            {
                hits[i].collider?.GetComponent<Enemy>().FocusTarget(_target);
            }
        }
    }
}
