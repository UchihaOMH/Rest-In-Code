using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerAnimState
{
    public override void InputProcess(GameObject input, Touch touch)
    {
        return;
    }
    public override void CurrentState()
    {
        Debug.Log("Die");
    }

    public void Die()
    {
        controller.targetAnim.SetBool(GameConst.AnimationParameter.bAlive, false);
    }
    public void Resurrect()
    {
        controller.targetAnim.SetBool(GameConst.AnimationParameter.bAlive, true);
        controller.State = controller.animStruct.run;
    }
}
