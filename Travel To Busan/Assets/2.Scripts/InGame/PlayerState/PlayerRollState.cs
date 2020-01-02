using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerAnimState
{
    private bool onRoll = false;

    public override void InputProcess(GameObject input, Touch touch)
    {
        if (input == null)
        {
            TransitionProcess(controller.animStruct.run, input, touch);
            return;
        }

        if (!onRoll)
        {
            if (input == controller.downLeftButton)
            {
                Roll(Vector2.left);
            }
            else if (input == controller.downRightButton)
            {
                Roll(Vector2.right);
            }
            else
            {
                TransitionProcess(controller.animStruct.run, input, touch);
            }
        }
    }
    public override void CurrentState()
    {
        Debug.Log("Roll");
    }

    public void Roll(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return;

        controller.targetAnim.SetTrigger(GameConst.AnimationParameter.tRoll);
        onRoll = true;
        controller.LookAt(dir);
        controller.targetAnim.GetBehaviour<RollBehaviour>().Roll(dir, controller.targetTr, controller.speed, () =>
        {
            onRoll = false;
        });
    }
}
