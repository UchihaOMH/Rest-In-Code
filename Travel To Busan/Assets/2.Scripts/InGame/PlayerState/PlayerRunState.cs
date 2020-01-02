using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerAnimState
{
    public override void InputProcess(GameObject input, Touch touch)
    {
        if (input == null)
        {
            Run(Vector2.zero);
            return;
        }

        if (touch.phase == TouchPhase.Began)
        {
            //  공격
            if (input == controller.attackSymbol)
            {
                TransitionProcess(controller.animStruct.attack, input, touch);
            }
        }
        else
        {
            if (input == controller.leftButton)
            {
                Run(Vector2.left);
            }
            else if (input == controller.rightButton)
            {
                Run(Vector2.right);
            }
            else
            {
                //  점프
                if (input == controller.upButton || input == controller.upLeftButton || input == controller.upRightButton)
                {
                    TransitionProcess(controller.animStruct.jump, input, touch);
                }
                //  구르기
                else if (input == controller.downLeftButton || input == controller.downRightButton)
                {
                    TransitionProcess(controller.animStruct.roll, input, touch);
                }
                //  앉기
                else if (input == controller.downButton)
                {
                    TransitionProcess(controller.animStruct.sit, input, touch);
                }
                else
                {
                    Run(Vector2.zero);
                }
            }
        }
    }
    public override void CurrentState()
    {
        Debug.Log("Run");
    }

    public void Run(Vector2 dir)
    {
        if (dir.sqrMagnitude > 0.0f)
        {
            controller.targetTr.Translate(dir * controller.speed, Space.World);
            controller.targetAnim.SetFloat(GameConst.AnimationParameter.fRunBlend, 1.0f);
            controller.LookAt(dir);
        }
        else
            controller.targetAnim.SetFloat(GameConst.AnimationParameter.fRunBlend, 0.0f);
    }
}
