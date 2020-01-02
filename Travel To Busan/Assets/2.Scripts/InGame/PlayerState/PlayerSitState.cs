using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitState : PlayerAnimState
{
    public bool OnSit
    {
        get => onSit;
        private set
        {
            onSit = value;
            controller.targetAnim.SetBool(GameConst.AnimationParameter.bSit, value);
        }
    }

    private bool onSit = false;

    public override void InputProcess(GameObject input, Touch touch)
    {
        if (input == null)
        {
            TransitionProcess(controller.animStruct.run, input, touch);
            return;
        }

        if (touch.phase == TouchPhase.Began)
        {
            //  공격
            if (touch.phase == TouchPhase.Began && input == controller.attackSymbol)
            {
                TransitionProcess(controller.animStruct.attack, input, touch);
            }
        }
        else
        {
            if (input == controller.downButton)
            {
                Sit();
            }
            else
            {
                OnSit = false;

                //  달리기
                if (input == controller.leftButton || input == controller.rightButton)
                {
                    TransitionProcess(controller.animStruct.run, input, touch);
                }
                //  구르기
                else if (input == controller.downLeftButton || input == controller.downRightButton)
                {
                    TransitionProcess(controller.animStruct.roll, input, touch);
                }
                else
                {
                    TransitionProcess(controller.animStruct.run, input, touch);
                }
            }
        }
    }
    public override void CurrentState()
    {
        Debug.Log("Sit");
    }

    public void Sit()
    {
        OnSit = true;
    }
}
