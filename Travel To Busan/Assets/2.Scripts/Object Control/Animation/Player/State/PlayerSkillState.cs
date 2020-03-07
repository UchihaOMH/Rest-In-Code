using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState, IAnimState
{
    public float rizingAttackCoolTime = 2f;
    public float smashAttackCoolTime = 1f;

    public bool onRizingAttack = false;
    public bool onSmashAttack = false;

    private float rizingAttackTimer = 0f;
    private float smashAttackTimer = 0f;

    public string GetStateName()
    {
        return "Skill State";
    }
    public void Process()
    {
        KeyValuePair<string, Touch> currDir = player.inputModule.CurrDir;
        KeyValuePair<bool, Touch> skillButtonInput = player.inputModule.SkillButtonPressed;

        if (onRizingAttack)
        {
            return;
        }
        else
        {
            if (skillButtonInput.Key && skillButtonInput.Value.phase == TouchPhase.Began)
            {
                if (currDir.Key == "Up" || currDir.Key == "Up Left" || currDir.Key == "Up Right")
                {
                    if (!onRizingAttack && !onSmashAttack)
                        RizingAttack();
                }
                else if (currDir.Key == "Down" || currDir.Key == "Down Left" || currDir.Key == "Down Right")
                {
                    if (!onRizingAttack && !onSmashAttack)
                        SmashAttack();
                }
                else
                {
                    player.CurrState = player.animationStates.run;
                    return;
                }
            }
            else
            {
                player.CurrState = player.animationStates.run;
                return;
            }
        }
    }

    public void RizingAttack()
    {
        if (Time.time >= rizingAttackTimer)
        {
            player.apPortrait.Play(_PlayerAnimTrigger_.rizingAttack);
            rizingAttackTimer = Time.time + rizingAttackCoolTime;
            onRizingAttack = true;
        }
        else
            player.CurrState = player.animationStates.run;
    }
    public void SmashAttack()
    {
        if (Time.time >= smashAttackTimer)
        {
            player.apPortrait.Play(_PlayerAnimTrigger_.smashAttack);
            smashAttackTimer = Time.time + smashAttackCoolTime;
            onSmashAttack = true;
        }
        else
            player.CurrState = player.animationStates.run;
    }

    #region Animation Event
    public void OnAttack()
    {
        if (player.apPortrait.IsPlaying(_PlayerAnimTrigger_.rizingAttack))
        {
            player.weapon.RizingAttack(player.info.damage);
        }
        else if (player.apPortrait.IsPlaying(_PlayerAnimTrigger_.smashAttack))
        {
            player.weapon.SmashAttack(player.info.damage);
        }
    }
    public void OnAttackExit()
    {
        if (player.apPortrait.IsPlaying(_PlayerAnimTrigger_.rizingAttack))
        {
            onRizingAttack = false;
            player.apPortrait.Play(_PlayerAnimTrigger_.idle);
            player.CurrState = player.animationStates.run;
        }
        else if (player.apPortrait.IsPlaying(_PlayerAnimTrigger_.smashAttack))
        {
            onSmashAttack = false;
            player.apPortrait.Play(_PlayerAnimTrigger_.idle);
            player.CurrState = player.animationStates.run;
        }
    }
    #endregion
}
