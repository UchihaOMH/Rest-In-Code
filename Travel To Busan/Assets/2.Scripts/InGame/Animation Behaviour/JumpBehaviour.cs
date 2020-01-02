using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Action callback;

    private bool onJump = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        rb = PlayerControl.Instance.targetRb;
        rb.AddForce(Vector2.up * PlayerControl.Instance.jumpForce, ForceMode2D.Impulse);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (rb.velocity.y < -0.1f)
        {
            animator.SetFloat(GameConst.AnimationParameter.fJumpBlend, 1.0f);
        }
        else if (rb.velocity.y > 0.1f)
        {
            animator.SetFloat(GameConst.AnimationParameter.fJumpBlend, 0.0f);
        }
        else
        {
            animator.SetFloat(GameConst.AnimationParameter.fJumpBlend, 0.0f);
            animator.SetBool(GameConst.AnimationParameter.bJump, false);
            onJump = false;
            callback?.Invoke();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
    }

    public void Jump(Action callback)
    {
        if (!onJump)
        {
            PlayerControl.Instance.targetAnim.SetBool(GameConst.AnimationParameter.bJump, true);
            this.callback = callback;
            onJump = true;
        }
    }
}
