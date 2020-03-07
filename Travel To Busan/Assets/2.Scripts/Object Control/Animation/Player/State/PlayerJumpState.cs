using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState, IAnimState
{
    public AudioClip clip;

    public int maxJumpCount = 1;
    public int currJumpCount = 0;

    public string GetStateName()
    {
        return "Jump State";
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
            if (player.onGround)
                Jump();
            else if (player.inputModule.JumpButtonPressed.Value.phase == TouchPhase.Began)
                Jump();
        }

        switch (player.inputModule.CurrDir.Key)
        {
            case "Up Left":
            case "Down Left":
            case "Left":
                AirWalk(Vector2.left);
                break;
            case "Up Right":
            case "Down Right":
            case "Right":
                AirWalk(Vector2.right);
                break;
        }

        if (player.rb.velocity.y > -0.001f && player.rb.velocity.y < 0.001f)
            player.TransitionProcess(player.animationStates.run);
    }
    public void Jump()
    {
        if (player.onGround || currJumpCount < maxJumpCount)
        {
            if (currJumpCount < maxJumpCount)
            {
                currJumpCount++;
                if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.jump))
                    player.apPortrait.Play(_PlayerAnimTrigger_.jump);
                player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
            }
            else if (player.onGround)
            {
                currJumpCount++;
                if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.jump))
                    player.apPortrait.Play(_PlayerAnimTrigger_.jump);
                player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    public void AirWalk(Vector2 dir)
    {
        player.tr.Translate(dir * player.info.speed * Time.deltaTime, Space.World);
        player.LookAt(dir);
    }
}
