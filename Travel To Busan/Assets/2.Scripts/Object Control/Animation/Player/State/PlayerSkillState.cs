using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState, IAnimState
{
    public string GetStateName()
    {
        return "Command State";
    }
    public void Process()
    {
        KeyValuePair<string, Touch> currDir = player.inputModule.CurrDir;
        KeyValuePair<bool, Touch> skillButtonInput = player.inputModule.SkillButtonPressed;

        if (skillButtonInput.Key && skillButtonInput.Value.phase == TouchPhase.Began
            && currDir.Key != "")
        {
            if (currDir.Key == "Up")
            {
                SkillAttack1();
            }
            else if (currDir.Key == "Left" || currDir.Key == "Right")
            {
                SkillAttack2();
            }
            else if (currDir.Key == "Down")
            {
                SkillAttack3();
            }
        }
        else
            player.TransitionProcess(player.animationStates.run);
    }

    public void SkillAttack1()
    {
        player.weapon.SkillAttack1(player.info.damage);
    }
    public void SkillAttack2()
    {
        player.weapon.SkillAttack2(player.info.damage);
    }
    public void SkillAttack3()
    {
        player.weapon.SkillAttack3(player.info.damage);
    }
}
