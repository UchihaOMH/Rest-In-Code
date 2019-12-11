using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSitState : MonoBehaviour, IPlayerAnimState
{
    public PlayerControl controller;

    public void InputProcess()
    {
        if (Input.touchCount == 0)
        {
            controller.targetAnim.SetBool(GameConst.AnimationParameter.bSit, false);
            controller.State = controller.move;
            return;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            List<RaycastResult> set = new List<RaycastResult>();
            controller.eventData.position = Input.GetTouch(i).position;
            controller.graphicRaycaster.Raycast(controller.eventData, set);
            GameObject obj = set[0].gameObject;

            if (obj.layer.Equals(LayerMask.GetMask(GameConst.LayerDefinition.controller)))
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began && obj.Equals(controller.attackSymbol))
                {
                    controller.State = controller.attack;
                    return;
                }

                if (obj.Equals(controller.downButton))
                {
                    Sit();
                }
                else
                {
                    controller.targetAnim.SetBool(GameConst.AnimationParameter.bSit, false);
                    controller.State = controller.move;
                    return;
                }
            }
        }
    }

    void Sit()
    {
        controller.targetAnim.SetBool(GameConst.AnimationParameter.bSit, true);
    }
}
