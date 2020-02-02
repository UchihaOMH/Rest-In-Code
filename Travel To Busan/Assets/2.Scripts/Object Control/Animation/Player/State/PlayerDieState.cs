using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }
    public void Process()
    {
        
    }
    public void Die()
    {
        player.isDead = true;
        player.anim.SetTrigger(_PlayerAnimTrigger_.tDie);
    }
    public void Resurrect()
    {
        player.isDead = false;
        player.anim.Play("Run Blend");
        player.TransitionProcess(player.animationStates.run);
    }

    public string GetStateName()
    {
        return "Die State";
    }
}
