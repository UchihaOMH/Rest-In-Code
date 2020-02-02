using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }
    private void Start()
    {
        player.anim.GetBehaviour<RollBehaviour>().AddUpdataEvent(() =>
        {
            float moveX = Mathf.Lerp(0f, player.rollSpeed, Time.deltaTime);
            player.tr.Translate(Vector3.left * moveX, Space.Self);
        });
        player.anim.GetBehaviour<RollBehaviour>().AddExitEvent(() =>
        {
            player.TransitionProcess(player.animationStates.run);
        });
    }

    public void Process()
    {
        
    }
    public void Roll()
    {
        player.anim.SetTrigger(_PlayerAnimTrigger_.tRoll);
    }

    public string GetStateName()
    {
        return "Roll State";
    }
}
