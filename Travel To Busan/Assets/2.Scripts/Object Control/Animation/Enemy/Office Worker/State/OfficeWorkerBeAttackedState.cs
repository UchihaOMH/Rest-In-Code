using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerBeAttackedState : OfficeWorkerState, IAnimState
{
    public string GetStateName()
    {
        return "Be Attacked";
    }
    public void Process()
    {
        
    }

    public void BeAttacked(Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.3f)
    {
        if (!officeWorker.isDead)
        {
            officeWorker.hpBar.HideBar(false);
            if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.beAttacked))
                officeWorker.apPortrait.Play(_OfficeWorkerAnimTrigger_.beAttacked);

            officeWorker.tr.Translate(_knockBackDir * _knockBackDist, Space.World);

            CancelInvoke("ResetStance");
            Invoke("ResetStance", _knockBackDuration);
        }
    }

    private void ResetStance()
    {
        officeWorker.TransitionProcess(officeWorker.animationState.trace);
    }
}
