using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSitState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }

    public string GetStateName()
    {
        return "Sit State";
    }

    public void Process()
    {
        if (Input.touchCount == 0 || player.CurrentDirection == "")
        {
            Sit(false);
            player.TransitionProcess(player.animationStates.run);
        }

        switch (player.CurrentDirection)
        {
            case "Left":
            case "Right":
                Sit(false);
                player.TransitionProcess(player.animationStates.run);
                break;
            case "Up":
            case "Up Right":
            case "Up Left":
                Sit(false);
                player.TransitionProcess(player.animationStates.jump);
                break;
            case "Down":
                Sit(true);
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
                    Sit(false);
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
                if (detectedObj == player.rollButton)
                {
                    Sit(false);
                    player.TransitionProcess(player.animationStates.roll);
                    (player.CurrState as PlayerRollState).Roll();
                    return;
                }
                else if (detectedObj == player.cmdButton)
                {
                    Sit(false);
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
    public void Sit(bool value)
    {
        player.anim.SetBool(_PlayerAnimTrigger_.bSit, value);
    }
}
