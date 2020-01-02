using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAnimState
{
    public bool OnJump
    {
        get => onJump;
        private set
        {
            onJump = value;
            controller.targetAnim.SetBool(GameConst.AnimationParameter.bJump, value);
        }
    }

    private bool onJump = false;

    public override void InputProcess(GameObject input, Touch touch)
    {
        if (input == null && !OnJump)
        {
            OnJump = false;
            TransitionProcess(controller.animStruct.run, input, touch);
            return;
        }

        BlendCheck();

        if (OnJump)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (input == controller.attackSymbol)
                {
                    OnJump = false;
                    TransitionProcess(controller.animStruct.attack, input, touch);
                }
            }
            else if (input == controller.leftButton)
            {
                AirWalk(Vector2.left);
            }
            else if (input == controller.rightButton)
            {
                AirWalk(Vector2.right);
            }
            else if (input == controller.upLeftButton)
            {
                AirWalk(Vector2.left);
            }
            else if (input == controller.upRightButton)
            {
                AirWalk(Vector2.right);
            }
        }
        else
        {
            if (input == controller.upButton)
            {
                Jump(input, touch);
            }
            else if (input == controller.upLeftButton)
            {
                Jump(input, touch);
                AirWalk(Vector2.left);
            }
            else if (input == controller.upRightButton)
            {
                Jump(input, touch);
                AirWalk(Vector2.right);
            }
            else
            {
                TransitionProcess(controller.animStruct.run, input, touch);
            }
        }
    }
    public override void CurrentState()
    {
        Debug.Log("Jump");
    }

    public void Jump(GameObject input, Touch touch)
    {
        OnJump = true;
        controller.targetRb.AddForce(Vector2.up * controller.jumpForce, ForceMode2D.Impulse);
    }
    public void AirWalk(Vector2 dir)
    {
        controller.targetTr.Translate(dir * controller.speed, Space.World);
        controller.LookAt(dir);
    }
    public void BlendCheck()
    {
        if (!OnJump)
            return;

        if (controller.targetRb.velocity.y < -0.5f)
        {
            OnJump = true;
            controller.targetAnim.SetFloat(GameConst.AnimationParameter.fJumpBlend, 1.0f);
        }
        else if (controller.targetRb.velocity.y > 0.5f)
        {
            OnJump = true;
            controller.targetAnim.SetFloat(GameConst.AnimationParameter.fJumpBlend, 0.0f);
        }
        else
        {
            Debug.Log("!Onjump");
            OnJump = false;
        }
    }
}
