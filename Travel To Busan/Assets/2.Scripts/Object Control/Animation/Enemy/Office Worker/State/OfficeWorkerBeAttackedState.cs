using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerBeAttackedState : OfficeWorkerState, IAnimState
{
    private float timer = -1f;

    public string GetStateName()
    {
        return "Be Attacked";
    }
    public void Process()
    {
        
    }

    public void BeAttacked(Vector2 _knockBackDir, float _knockBackPower, float _knockBackDuration = 0.3f)
    {
        if (!officeWorker.isDead)
        {
            officeWorker.hpBar.HideBar(false);
            officeWorker.apPortrait.Play(_OfficeWorkerAnimTrigger_.beAttacked);

            officeWorker.rb.AddForce(_knockBackDir.normalized * _knockBackPower, ForceMode2D.Impulse);

            CancelInvoke("ResetStance");
            Invoke("ResetStance", _knockBackDuration);
        }
    }

    private void ResetStance()
    {
        officeWorker.TransitionProcess(officeWorker.animationState.trace);
    }
}
