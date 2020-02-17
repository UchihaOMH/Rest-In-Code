using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState, IAnimState
{
    public AudioClip clip;

    public string GetStateName()
    {
        return "Base Attack State";
    }
    public void Process()
    {
        if (!player.onGround)
        {
            switch (player.inputModule.CurrDir.Key)
            {
                case "Left":
                    player.LookAt(Vector2.left);
                    player.tr.Translate(Vector2.left * player.info.speed, Space.World);
                    break;
                case "Right":
                    player.LookAt(Vector2.right);
                    player.tr.Translate(Vector2.right * player.info.speed, Space.World);
                    break;
            }
        }
    }

    public void Attack()
    {
        player.apPortrait.CrossFade(_PlayerAnimTrigger_.attack, 0.1f);
        player.apPortrait.SetAnimationSpeed(_PlayerAnimTrigger_.attack, 1f);
        player.weapon.Attack(player.info.damage);
    }

    #region Animation Event
    public void OnAttackExit()
    {
        player.apPortrait.Play(_PlayerAnimTrigger_.idle);
        player.TransitionProcess(player.animationStates.run);
    }
    public void OnSmashExit()
    {
        player.apPortrait.Play(_PlayerAnimTrigger_.idle);
        player.TransitionProcess(player.animationStates.run);
    }
    public void OnRizingAttackExit()
    {
        player.apPortrait.Play(_PlayerAnimTrigger_.idle);
        player.TransitionProcess(player.animationStates.run);
    }
    #endregion
}
