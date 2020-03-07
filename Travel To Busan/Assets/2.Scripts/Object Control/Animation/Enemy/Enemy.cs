using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity, IManagedObject
{
    protected Entity target;

    public abstract Transform Pool
    {
        get;
        set;
    }
    protected Transform pool;

    public abstract void FocusTarget(Entity _target);
    public abstract void ResetObjectForPooling();
    public abstract void ReturnObject2Pool();
}
