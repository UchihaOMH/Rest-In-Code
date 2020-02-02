using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRunState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }
    public string GetStateName()
    {
        return "Run State";
    }

    public void Process()
    {
        if (Input.touchCount == 0 || player.CurrentDirection == "")
        {
            Run(Vector2.zero, 0f);
        }

        switch (player.CurrentDirection)
        {
            case "Left":
                Run(Vector2.left, 1f);
                break;
            case "Right":
                Run(Vector2.right, 1f);
                break;
            case "Up":
            case "Up Right":
            case "Up Left":
                Run(Vector2.zero, 0f);
                player.TransitionProcess(player.animationStates.jump);
                break;
            case "Down":
                Run(Vector2.zero, 0f);
                player.TransitionProcess(player.animationStates.sit);
                break;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            List<RaycastResult> set = new List<RaycastResult>();
            player.eventData.position = touch.position;
            player.raycaster.Raycast(player.eventData, set);

            try
            {
                GameObject detectedObj = set[0].gameObject;

                if (detectedObj == player.attackButton)
                {
                    Run(Vector2.zero, 0f);
                    if (!player.onGround)
                    {
                        player.TransitionProcess(player.animationStates.jumpAttack);
                        (player.CurrState as PlayerJumpAttackState).JumpAttack();
                        return;
                    }
                    else
                    {
                        player.TransitionProcess(player.animationStates.baseAttack);
                        (player.CurrState as PlayerBaseAttackState).Attack();
                        return;
                    }
                }
                else if (detectedObj == player.rollButton)
                {
                    Run(Vector2.zero, 0f);
                    player.TransitionProcess(player.animationStates.roll);
                    (player.CurrState as PlayerRollState).Roll();
                    return;
                }
                else if (detectedObj == player.cmdButton)
                {
                    Run(Vector2.zero, 0f);
                    player.TransitionProcess(player.animationStates.cmd);
                    return;

                }
            }
            catch (System.Exception)
            {
                continue;
            }
        }
    }
    public void Run(Vector2 dir, float blend)
    {
        if (dir != Vector2.zero)
        {
            player.tr.Translate(dir * player.info.speed, Space.World);
            player.LookAt(dir);
        }

        player.anim.SetFloat(_PlayerAnimTrigger_.fRunBlend, blend);
    }
}
