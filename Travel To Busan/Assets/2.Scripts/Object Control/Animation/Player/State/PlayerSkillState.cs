using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState, IAnimState
{
    public string GetStateName()
    {
        return "Command State";
    }
    public void Process()
    {
        KeyValuePair<string, Touch> currDir = player.inputModule.CurrDir;
        KeyValuePair<bool, Touch> skillButtonInput = player.inputModule.SkillButtonPressed;

        if (skillButtonInput.Key && skillButtonInput.Value.phase == TouchPhase.Began)
        {
            if (currDir.Key == "Up")
            {
                SkillAttack1();
            }
            else if (currDir.Key == "Down")
            {
                SkillAttack2();
            }
            else if (currDir.Key == "")
                player.TransitionProcess(player.animationStates.run);
        }
    }

    public void SkillAttack1()
    {
        player.apPortrait.Play(_PlayerAnimTrigger_.rizingAttack);
    }
    public void SkillAttack2()
    {
        player.apPortrait.Play(_PlayerAnimTrigger_.smashAttack);
    }

    #region Animation Event
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
    public void OnSmashAttack()
    {
        player.weapon.SmashAttack(player.info.damage);
    }
    public void OnRizingAttack()
    {
        player.weapon.RizingAttack(player.info.damage);
    }
    #endregion
}
