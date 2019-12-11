using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJumpState : MonoBehaviour, IPlayerAnimState
{
    public PlayerControl controller;

    private bool onJump = false;

    public void InputProcess()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            List<RaycastResult> set = new List<RaycastResult>();
            controller.eventData.position = Input.GetTouch(i).position;
            controller.graphicRaycaster.Raycast(controller.eventData, set);
            GameObject obj = set[0].gameObject;

            if (obj.layer.Equals(LayerMask.GetMask(GameConst.LayerDefinition.controller)))
            {
                if (onJump)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began && obj.Equals(controller.attackSymbol))
                    {
                        controller.State = controller.attack;
                        return;
                    }

                    if (obj.Equals(controller.leftButton) || obj.Equals(controller.upLeftButton))
                    {
                        AirMove(Vector2.left);
                    }
                    else if (obj.Equals(controller.rightButton) || obj.Equals(controller.upRightButton))
                    {
                        AirMove(Vector2.right);
                    }
                }
                else
                {
                    if (obj.Equals(controller.upButton))
                    {
                        Jump(0.0f);
                    }
                    else if (obj.Equals(controller.upLeftButton))
                    {
                        Jump(0.0f);
                        AirMove(Vector2.left);
                    }
                    else if (obj.Equals(controller.upRightButton))
                    {
                        Jump(0.0f);
                        AirMove(Vector2.right);
                    }
                }
            }
            else
                continue;
        }
    }

    void AirMove(Vector2 dir)
    {
        controller.LookAt(dir);
        controller.targetTr.Translate(dir * controller.moveSpeed, Space.World);
    }
    void Jump(float blend)
    {
        if (!onJump)
        {
            controller.targetAnim.SetBool(GameConst.AnimationParameter.bJump, true);
            controller.targetAnim.SetFloat(GameConst.AnimationParameter.fJumpBlend, blend);
            controller.targetRb.AddForce(Vector2.up * controller.jumpForce, ForceMode2D.Impulse);
            StartCoroutine(JumpCoroutine());
        }
    }
    IEnumerator JumpCoroutine()
    {
        onJump = true;

        while (onJump)
        {
            if (controller.targetRb.velocity.y < -0.1f)
            {
                controller.targetAnim.SetFloat(GameConst.AnimationParameter.fJumpBlend, 1.0f);
            }
            else if (controller.targetRb.velocity.y < 0.1f)
            {
                onJump = false;
                controller.targetAnim.SetBool(GameConst.AnimationParameter.bJump, false);
                controller.State = controller.move;
            }

            yield return null;
        }
    }
}
