using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.EventSystems;

public class PlayerDieState : MonoBehaviour, IPlayerAnimState
{
    public PlayerControl controller;

    public void InputProcess()
    {
        Die();

        //for (int i = 0; i < Input.touchCount; i++)
        //{
        //    List<RaycastResult> set = new List<RaycastResult>();
        //    controller.eventData.position = Input.GetTouch(i).position;
        //    controller.graphicRaycaster.Raycast(controller.eventData, set);
        //    GameObject obj = set[0].gameObject;

        //    if (obj.layer.Equals(LayerMask.GetMask(GameConst.LayerDefinition.controller)))
        //    {

        //    }
        //}
    }

    void Die()
    {
        controller.targetAnim.SetTrigger(GameConst.AnimationParameter.tDie);
        controller.isDead = true;
    }
}
