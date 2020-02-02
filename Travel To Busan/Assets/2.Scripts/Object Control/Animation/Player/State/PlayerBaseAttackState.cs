using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseAttackState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }
    private void Start()
    {
        player.anim.GetBehaviour<BaseAttackBehaviour>().AddExitEvent(() =>
        {
            player.TransitionProcess(player.animationStates.run);
        });
    }

    public void Process()
    {
        
    }
    public void Attack()
    {
        player.anim.SetTrigger(_PlayerAnimTrigger_.tBaseAttack);
        player.weapon.Attack(player.info.damage);
    }

    public string GetStateName()
    {
        return "Base Attack State";
    }
}
