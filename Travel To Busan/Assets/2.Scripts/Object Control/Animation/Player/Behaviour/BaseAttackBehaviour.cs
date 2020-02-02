using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackBehaviour : AnimationBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        enterCallback?.Invoke();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        exitCallback?.Invoke();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        updateCallback?.Invoke();
    }
}
