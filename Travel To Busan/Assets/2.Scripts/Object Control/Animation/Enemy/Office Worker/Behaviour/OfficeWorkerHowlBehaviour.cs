using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerHowlBehaviour : AnimationBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        exitCallback?.Invoke();
    }
}
