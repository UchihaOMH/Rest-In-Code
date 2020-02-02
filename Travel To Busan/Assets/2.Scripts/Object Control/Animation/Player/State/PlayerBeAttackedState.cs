using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeAttackedState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }
    private void Awake()
    {
        player.anim.GetBehaviour<BeAttackedBehaviour>().AddExitEvent(() =>
        {
            player.TransitionProcess(player.animationStates.run);
        });
    }

    public void Process()
    {

    }
    public void BeAttacked()
    {
        player.anim.SetTrigger(_PlayerAnimTrigger_.tBeAttacked);
    }

    public string GetStateName()
    {
        return "Be Attacked State";
    }
}
