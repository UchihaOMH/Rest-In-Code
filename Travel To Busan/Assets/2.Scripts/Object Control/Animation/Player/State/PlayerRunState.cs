using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState, IAnimState
{
    public AudioClip clip;

    public string GetStateName()
    {
        return "Run State";
    }
    public void Process()
    {
        if (player.inputModule.AttackButtonPressed.Key)
        {
            player.TransitionProcess(player.animationStates.attack);
            (player.CurrState as PlayerAttackState).Attack();
        }
        else if (player.inputModule.SkillButtonPressed.Key && player.inputModule.SkillButtonPressed.Value.phase == TouchPhase.Began)
        {
            player.TransitionProcess(player.animationStates.skill);
        }
        else if (player.inputModule.JumpButtonPressed.Key)
        {
            player.TransitionProcess(player.animationStates.jump);
        }
        else
        {
            switch (player.inputModule.CurrDir.Key)
            {
                case "Up Left":
                case "Down Left":
                case "Left":
                    Run(Vector2.left);
                    break;
                case "Up Right":
                case "Down Right":
                case "Right":
                    Run(Vector2.right);
                    break;

                case "":
                    if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.idle))
                        player.apPortrait.Play(_PlayerAnimTrigger_.idle);
                    break;
            }
        }
    }

    public void Run(Vector2 dir)
    {
        if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.run))
            player.apPortrait.Play(_PlayerAnimTrigger_.run);
        player.tr.Translate(dir * player.info.speed * Time.deltaTime, Space.World);
        player.LookAt(dir);
    }
}
