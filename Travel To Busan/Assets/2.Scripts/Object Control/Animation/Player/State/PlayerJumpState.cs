using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJumpState : PlayerState, IAnimState
{
    public int maxJumpCount = 1;
    public int currJumpCount = 0;

    public PlayerState GetPlayerState()
    {
        return this;
    }
    public void Process()
    {
        switch (player.CurrentDirection)
        {
            case "Up":
                if (!player.onGround && player.GetControlPadInput().Value == TouchPhase.Began)
                    Jump();
                else
                    Jump();
                break;
            case "Up Left":
            case "Left":
                if (!player.onGround && player.GetControlPadInput().Value == TouchPhase.Began)
                    Jump();
                else
                    Jump();
                AirWalk(Vector2.left);
                break;
            case "Up Right":
            case "Right":
                if (!player.onGround && player.GetControlPadInput().Value == TouchPhase.Began)
                    Jump();
                else
                    Jump();
                AirWalk(Vector2.right);
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
                    player.anim.SetBool(_PlayerAnimTrigger_.bJump, false);
                    if (!player.onGround)
                    {
                        player.anim.SetBool(_PlayerAnimTrigger_.bJump, true);
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
                if (detectedObj == player.cmdButton)
                {
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
    public void Jump()
    {
        if (player.onGround || currJumpCount < maxJumpCount)
        {
            if (currJumpCount > 0 && player.GetControlPadInput().Value == TouchPhase.Began)
            {
                currJumpCount++;
                player.anim.SetBool(_PlayerAnimTrigger_.bJump, true);
                player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                currJumpCount++;
                player.anim.SetBool(_PlayerAnimTrigger_.bJump, true);
                player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
            }
        }
    }
    public void AirWalk(Vector2 dir)
    {
        player.tr.Translate(dir * player.info.speed, Space.World);
        player.LookAt(dir);
    }

    public string GetStateName()
    {
        return "Jump State";
    }
}
