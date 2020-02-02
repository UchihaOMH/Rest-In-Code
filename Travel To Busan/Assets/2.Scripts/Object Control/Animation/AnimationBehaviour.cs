using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationBehaviour : StateMachineBehaviour
{
    protected Action enterCallback;
    protected Action updateCallback;
    protected Action exitCallback;

    public void AddEnterEvent(Action _callback)
    {
        enterCallback = _callback;
    }
    public void AddExitEvent(Action _callback)
    {
        exitCallback = _callback;
    }
    public void AddUpdataEvent(Action _callback)
    {
        updateCallback = _callback;
    }
}
