using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerAnimState
{
    protected PlayerControl controller = PlayerControl.Instance;

    public virtual void InputProcess(GameObject input, Touch touch)
    {
        return;
    }
    public virtual void CurrentState()
    {
        return;
    }

    public void TransitionProcess(PlayerAnimState state, GameObject input, Touch touch)
    {
        controller.State = state;
        controller.State.InputProcess(input, touch);
    }
}
