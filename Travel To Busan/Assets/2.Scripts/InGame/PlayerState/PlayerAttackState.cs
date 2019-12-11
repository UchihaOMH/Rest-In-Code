using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackState : MonoBehaviour, IPlayerAnimState
{
    public PlayerControl controller;

    private Stack<GameObject> attackBuff;
    private bool onAttack = false;
    private int comboCount = 0;

    private Stack<GameObject> leftAttack;
    private Stack<GameObject> rightAttack;

    private void Start()
    {
        leftAttack = new Stack<GameObject>(new List<GameObject>() { controller.leftButton });
        rightAttack = new Stack<GameObject>(new List<GameObject>() { controller.rightButton });
    }

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
                if (Input.GetTouch(i).phase == TouchPhase.Began && obj.Equals(controller.attackSymbol))
                {
                    attackBuff = new Stack<GameObject>();
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Stationary)
                {
                    if (attackBuff.Peek() != obj)
                    {
                        attackBuff.Push(obj);
                    }

                    if (attackBuff.Equals(leftAttack))
                    {
                        controller.targetAnim.SetTrigger(GameConst.AnimationParameter.tAttack);
                        BasicCombo(Vector2.left);
                    }
                    else if (attackBuff.Equals(rightAttack))
                    {
                        controller.targetAnim.SetTrigger(GameConst.AnimationParameter.tAttack);
                        BasicCombo(Vector2.right);
                    }
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {

                    //  콤보 기술들 버퍼랑 비교해서 발동
                }
            }
        }
    }

    void BasicCombo(Vector2 dir)
    {
        comboCount %= 4 + 1;
        controller.LookAt(dir);
        controller.targetAnim.SetInteger(GameConst.AnimationParameter.iComboCount, comboCount);
    }
}
