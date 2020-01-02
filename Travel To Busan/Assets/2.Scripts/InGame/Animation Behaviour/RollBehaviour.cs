using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private bool onRoll = false;

    private Vector2 dir;
    private Transform tr;
    private float speed;

    private Action callback;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (onRoll)
        {
            tr.Translate(dir * speed, Space.World);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        callback?.Invoke();

        onRoll = false;
    }

    public void Roll(Vector2 dir, Transform tr, float speed, Action animationEndCallback)
    {
        if (!onRoll)
        {
            this.dir = dir;
            this.tr = tr;
            this.speed = speed;
            callback = animationEndCallback;

            onRoll = true;
        }
    }
}
