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

        if (skillButtonInput.Key && skillButtonInput.Value.phase == TouchPhase.Began)
        {
            if (currDir.Key == "Up")
            {
                RizingAttack();
            }
            else if (currDir.Key == "Down")
            {
                SmashAttack();
            }
            else if (currDir.Key == "")
                player.TransitionProcess(player.animationStates.run);
        }

        if (onRizingAttack)
        {
            switch (player.inputModule.CurrDir.Key)
            {
                case "Left":
                    player.tr.Translate(Vector3.left * player.info.speed * Time.deltaTime, Space.World);
                    break;
                case "Right":
                    player.tr.Translate(Vector3.right * player.info.speed * Time.deltaTime, Space.World);
                    break;
            }
        }
    }

    public void RizingAttack()
    {
        if (Time.time >= rizingAttackTimer)
        {
            player.apPortrait.Play(_PlayerAnimTrigger_.rizingAttack);
            rizingAttackTimer = Time.time + rizingAttackCoolTime;
        }
    }
    public void SmashAttack()
    {
        if (Time.time >= smashAttackTimer)
        {
            player.apPortrait.Play(_PlayerAnimTrigger_.smashAttack);
            smashAttackTimer = Time.time + smashAttackCoolTime;
        }
    }

    #region Animation Event
    public void OnSmashExit()
    {
        onSmashAttack = false;
        player.apPortrait.Play(_PlayerAnimTrigger_.idle);
        player.TransitionProcess(player.animationStates.run);
    }
    public void OnRizingAttackExit()
    {
        onRizingAttack = false;
        player.apPortrait.Play(_PlayerAnimTrigger_.idle);
        player.TransitionProcess(player.animationStates.run);
    }
    public void OnSmashAttack()
    {
        onSmashAttack = true;
        player.weapon.SmashAttack(player.info.damage);
    }
    public void OnRizingAttack()
    {
        onRizingAttack = true;
        player.weapon.RizingAttack(player.info.damage);
    }
    #endregion
}
