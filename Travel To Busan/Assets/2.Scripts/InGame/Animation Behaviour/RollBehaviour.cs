using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private bool onRoll = false;

    private Vector2 dir;
    private Transform tr;
    private float speed;

    System.Action callback;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (onRoll)
        {
            tr.Translate(dir * speed, Space.World);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        callback();

        onRoll = false;

        dir = Vector2.zero;
        tr = null;
        speed = 0.0f;

        callback = null;
    }

    public void Roll(Vector2 _dir, Transform _tr, float _speed, System.Action animationEndCallback)
    {
        if (!onRoll)
        {
            dir = _dir;
            tr = _tr;
            speed = _speed;
            callback = animationEndCallback;

            onRoll = true;
        }
    }
}
