using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRollState : MonoBehaviour, IPlayerAnimState
{
    public PlayerControl controller;

    private bool onRoll = false;

    public void InputProcess()
    {
        Roll(Vector2.right);

        for (int i = 0; i < Input.touchCount; i++)
        {
            List<RaycastResult> set = new List<RaycastResult>();
            controller.eventData.position = Input.GetTouch(i).position;
            controller.graphicRaycaster.Raycast(controller.eventData, set);
            GameObject obj = set[0].gameObject;

            if (obj.layer.Equals(LayerMask.GetMask(GameConst.LayerDefinition.controller)))
            {
                if (!onRoll)
                {
                    if (obj.Equals(controller.downLeftButton))
                    {
                        Roll(Vector2.left);
                    }
                    else if (obj.Equals(controller.downRightButton))
                    {
                        Roll(Vector2.right);
                    }
                }
            }
        }
    }

    void Roll(Vector2 dir)
    {
        controller.targetAnim.SetTrigger(GameConst.AnimationParameter.tRoll);
        controller.LookAt(dir);
        onRoll = true;

        controller.targetAnim.GetBehaviour<RollBehaviour>().Roll(dir, controller.targetTr, controller.moveSpeed, () =>
        {
            onRoll = false;
            controller.State = controller.move;
            return;
        });
    }
}
