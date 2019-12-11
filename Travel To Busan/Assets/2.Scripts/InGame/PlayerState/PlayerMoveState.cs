using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveState : MonoBehaviour, IPlayerAnimState
{
    public PlayerControl controller;

    public void InputProcess()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            List<RaycastResult> set = new List<RaycastResult>();
            controller.eventData.position = Input.GetTouch(i).position;
            controller.graphicRaycaster.Raycast(controller.eventData, set);
            if (set == null)
                return;

            GameObject obj = set[0].gameObject;
            Debug.Log(LayerMask.LayerToName(obj.layer) == GameConst.LayerDefinition.controller);

            if (LayerMask.LayerToName(obj.layer) == GameConst.LayerDefinition.controller)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began && obj.Equals(controller.attackSymbol))
                {
                    controller.targetAnim.SetFloat(GameConst.AnimationParameter.fMoveBlend, 0.0f);
                    controller.State = controller.attack;
                    return;
                }

                if (obj.Equals(controller.upButton) || obj.Equals(controller.upLeftButton) || obj.Equals(controller.upRightButton))
                {
                    controller.targetAnim.SetFloat(GameConst.AnimationParameter.fMoveBlend, 0.0f);
                    controller.State = controller.jump;
                    return;
                }
                else if (obj.Equals(controller.leftButton))
                {
                    Move(1.0f, Vector2.left);
                }
                else if (obj.Equals(controller.rightButton))
                {
                    Move(1.0f, Vector2.right);
                }
                else if (obj.Equals(controller.downButton))
                {
                    controller.targetAnim.SetFloat(GameConst.AnimationParameter.fMoveBlend, 0.0f);
                    controller.State = controller.sit;
                    return;
                }
                else if (obj.Equals(controller.downLeftButton) || obj.Equals(controller.downRightButton))
                {
                    controller.targetAnim.SetFloat(GameConst.AnimationParameter.fMoveBlend, 0.0f);
                    controller.State = controller.roll;
                    return;
                }
            }
            else
                continue;
        }
    }

    void Move(float blend, Vector2 dir)
    {
        controller.targetAnim.SetFloat(GameConst.AnimationParameter.fMoveBlend, blend);
        if (blend < -0.1f && blend > 0.1f)
        {
            controller.LookAt(dir);
            controller.targetTr.Translate(dir * controller.moveSpeed, Space.World);
        }
    }
}
