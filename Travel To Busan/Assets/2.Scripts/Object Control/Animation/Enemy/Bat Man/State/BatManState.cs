using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BatManState : MonoBehaviour, IAnimState
{
    public BatMan batMan;

    public Transform Pool
    {
        get => pool;
        set => pool = value;
    }
    protected Transform pool;

    public abstract string GetStateName();
    public abstract void Process();
}
