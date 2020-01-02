using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAnimState
{
    public bool OnAttack
    {
        get => onAttack;
        private set
        {
            controller.targetAnim.SetBool(GameConst.AnimationParameter.bAttack, value);
            onAttack = value;
        }
    }
    public bool OnBasicAttack 
    {
        get => onBasicAttack;
        private set
        {
            controller.targetAnim.SetBool(GameConst.AnimationParameter.bBasicAttack, value);
            onBasicAttack = value;
        }
    }

    private Stack<GameObject> buff;
    private Stack<GameObject> leftBasicAttack;
    private Stack<GameObject> rightBasicAttack;
    private Stack<GameObject> leftUpperCommand;
    private Stack<GameObject> rightUpperCommand;
    private Stack<GameObject> leftLowerCommand;
    private Stack<GameObject> rightLowerCommand;
    private Stack<GameObject> breakCommand;

    private bool onAttack = false;
    private bool onBasicAttack = false;

    public PlayerAttackState()
    {
        leftBasicAttack = new Stack<GameObject>(new List<GameObject>()
        { controller.leftButton });
        rightBasicAttack = new Stack<GameObject>(new List<GameObject>()
        { controller.leftButton });

        leftUpperCommand = new Stack<GameObject>(new List<GameObject>()
        { controller.leftButton, controller.upLeftButton });
        rightUpperCommand = new Stack<GameObject>(new List<GameObject>()
        { controller.rightButton, controller.upRightButton });

        leftLowerCommand = new Stack<GameObject>(new List<GameObject>()
        { controller.leftButton, controller.downLeftButton });
        rightLowerCommand = new Stack<GameObject>(new List<GameObject>()
        { controller.rightButton, controller.downRightButton });

        breakCommand = new Stack<GameObject>(new List<GameObject>()
        { controller.downButton });
    }

    public override void InputProcess(GameObject input, Touch touch)
    {
        if (input == null)
        {
            OnAttack = false;
            OnBasicAttack = false;
            TransitionProcess(controller.animStruct.run, input, touch);
            return;
        }

        if (touch.phase == TouchPhase.Began)
        {
            onAttack = true;
            buff = new Stack<GameObject>();
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            PushToBuffer(input);
        }
        else if (touch.phase == TouchPhase.Stationary)
        {
            if (CompareBuffer(buff, leftBasicAttack))
            {
                OnBasicAttack = true;
                controller.LookAt(Vector2.left);
            }
            else if (CompareBuffer(buff, rightBasicAttack))
            {
                OnBasicAttack = true;
                controller.LookAt(Vector2.right);
            }
        }
        else
        {
            if (controller.targetRb.velocity.y < -0.1f)
            {
                if (CompareBuffer(buff, breakCommand))
                {

                }
            }
            else if (CompareBuffer(buff, rightUpperCommand))
            {

            }
            else if (CompareBuffer(buff, leftUpperCommand))
            {

            }
            else if (CompareBuffer(buff, rightLowerCommand))
            {

            }
            else if (CompareBuffer(buff, leftLowerCommand))
            {

            }

            OnAttack = false;
            OnBasicAttack = false;
            TransitionProcess(controller.animStruct.run, input, touch);
        }
    }
    public override void CurrentState()
    {
        Debug.Log("Attack");
    }

    public void PushToBuffer(GameObject input)
    {
        if (buff.Count == 0)
            buff.Push(input);
        else if (buff.Peek() != input && input != controller.attackSymbol)
            buff.Push(input);
    }
    public bool CompareBuffer(Stack<GameObject> buff_1, Stack<GameObject> buff_2)
    {
        Stack<GameObject> a = new Stack<GameObject>(buff_1);
        Stack<GameObject> b = new Stack<GameObject>(buff_2);

        if (a.Count != 0 && a.Count == b.Count)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (a.Pop() != b.Pop())
                    return false;
            }
            return true;
        }
        return false;
    }
}
