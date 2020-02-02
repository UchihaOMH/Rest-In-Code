using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }
    private void Awake()
    {
        player.anim.GetBehaviour<JumpAttackBehaviour>().AddExitEvent(() =>
        {
            player.TransitionProcess(player.animationStates.run);
        });
    }

    public string GetStateName()
    {
        return "Player Jump Attack State";
    }

    public void Process()
    {
        switch (player.CurrentDirection)
        {
            case "Up Left":
            case "Left":
                player.tr.Translate(Vector2.left * player.info.speed, Space.World);
                break;
            case "Right":
            case "Up Right":
                player.tr.Translate(Vector2.right * player.info.speed, Space.World);
                break;
        }
    }
    public void JumpAttack()
    {
        player.anim.SetTrigger(_PlayerAnimTrigger_.tJumpAttack);
        player.weapon.Attack(player.info.damage);
    }
}
