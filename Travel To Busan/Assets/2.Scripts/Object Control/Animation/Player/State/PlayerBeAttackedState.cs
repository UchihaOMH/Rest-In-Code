using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeAttackedState : PlayerState, IAnimState
{
    public void Process()
    {
        
    }
    public void BeAttacked(Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.1f)
    {
        player.apPortrait.Play(_PlayerAnimTrigger_.beAttacked);
        player.tr.Translate(_knockBackDir.normalized * _knockBackDist, Space.World);
        CancelInvoke("ResetStance");
        Invoke("ResetStance", _knockBackDuration);
    }

    private void ResetStance()
    {
        if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.die) && player.CurrState == this)
        {
            player.apPortrait.Play(_PlayerAnimTrigger_.idle);
            player.TransitionProcess(player.animationStates.run);
        }
    }

    public string GetStateName()
    {
        return "Be Attacked State";
    }
}
