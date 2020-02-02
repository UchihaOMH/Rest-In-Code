using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCmdState : PlayerState, IAnimState
{
    public PlayerState GetPlayerState()
    {
        return this;
    }
    /// <summary>
    /// 점프중에 쓰면 내려찍기, 땅에서 쓰면 버프
    /// </summary>
    public Stack<string> Cmd1
    {
        get => cmd1;
        set => cmd1 = value;
    }
    private Stack<string> cmd1;

    /// <summary>
    /// 돌진기 타입 공격기
    /// </summary>
    public Stack<string> Cmd2
    {
        get => cmd2;
        set => cmd2 = value;
    }
    private Stack<string> cmd2;

    /// <summary>
    /// 기동력 타입 유틸기
    /// </summary>
    public Stack<string> Cmd3
    {
        get => cmd3;
        set => cmd3 = value;
    }
    private Stack<string> cmd3;
    private Stack<string> buff;
    
    public void Process()
    {
        var inputPair = player.GetControlPadInput();

        if (inputPair.Value == TouchPhase.Began)
            buff = new Stack<string>();

        if (inputPair.Value == TouchPhase.Ended)
            CommandAttack();
        else
            PushToBuffer(inputPair.Key);
    }
    public void PushToBuffer(string _data)
    {
        if (buff.Count == 0)
            buff.Push(_data);
        else if (buff.Peek() != _data)
        {
            buff.Push(_data);
        }
    }
    public void CommandAttack()
    {
        if (CompareStack(buff, Cmd1))
        {
            if (!player.onGround)
            {
                DropAttack();
            }
            else
            {
                Buff();
            }
        }
        else if (CompareStack(buff, cmd2))
        {
            DashAttack();
        }
        else if (CompareStack(buff, cmd3))
        {
            RisingUpper();
        }
        else
        {
            player.TransitionProcess(player.animationStates.run);
        }
    }
    public bool CompareStack(Stack<string> _a, Stack<string> _b)
    {
        try
        {
            Stack<string> a = new Stack<string>(_a);
            Stack<string> b = new Stack<string>(_b);

            if (a.Count == 0)
                return false;
            else if (a.Count != b.Count)
                return false;
            else
            {
                for (int i = 0; i < a.Count; i++)
                {
                    if (a.Pop() != b.Pop())
                        return false;
                }
                return true;
            }
        }
        catch (System.Exception)
        {
            return false;
        }
    }
    public void DropAttack()
    {
    
    }
    public void Buff()
    {
    
    }
    public void DashAttack()
    {
    
    }
    public void RisingUpper()
    {
    
    }

    public string GetStateName()
    {
        return "Command State";
    }
}
