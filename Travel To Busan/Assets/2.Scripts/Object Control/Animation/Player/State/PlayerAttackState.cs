using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState, IAnimState
{
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
                    player.tr.Translate(Vector2.left * player.info.speed * Time.deltaTime, Space.World);
                    break;
                case "Right":
                    player.tr.Translate(Vector2.right * player.info.speed * Time.deltaTime, Space.World);
                    break;
            }
        }
    }

    public void Attack()
    {
        player.apPortrait.SetAnimationSpeed(_OfficeWorkerAnimTrigger_.attack, 1.5f);
        player.apPortrait.Play(_PlayerAnimTrigger_.attack);
        player.apPortrait.SetAnimationSpeed(_PlayerAnimTrigger_.attack, 1f);
        player.weapon.Attack(player.info.damage);
    }

    #region Animation Event
    public void OnAttackExit()
    {
        player.apPortrait.Play(_PlayerAnimTrigger_.idle);
        player.TransitionProcess(player.animationStates.run);
    }
    #endregion
}
