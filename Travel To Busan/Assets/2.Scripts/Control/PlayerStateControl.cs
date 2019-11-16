using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션 함수 제공
/// 애니메이터의 파라미터 조정
/// </summary>
public class PlayerStateControl
{
    #region Property
    public GameConst.AnimationParam.eState motionState
    {
        get => _motionState;
        private set
        {
            previousMotionState = motionState;
            _motionState = value;
            OnStateChanged();
        }
    }
    public GameConst.AnimationParam.eState previousMotionState
    {
        get => _previousMotionState;
        private set => _previousMotionState = value;
    }
    #endregion
    
    public Animator animator;
    public int comboCount = 0;
    public float comboAvailableTime = 0f;
    public float comboAvailableInterval = 0.7f;

    private GameConst.AnimationParam.eState _motionState = GameConst.AnimationParam.eState.Idle;
    private GameConst.AnimationParam.eState _previousMotionState;

    public PlayerStateControl() { }
    public PlayerStateControl(Animator _animator)
    {
        animator = _animator;
    }

    #region Public Method
    public void Run()
    {
        if (motionState != GameConst.AnimationParam.eState.Run)
            motionState = GameConst.AnimationParam.eState.Run;
    }
    public void Idle()
    {
        if (motionState != GameConst.AnimationParam.eState.Idle)
            motionState = GameConst.AnimationParam.eState.Idle;
    }
    public void Die()
    {
        
        motionState = GameConst.AnimationParam.eState.Die;
    }
    public void Jump()
    {
        motionState = GameConst.AnimationParam.eState.Jump;
    }
    public void Fall()
    {
        if (motionState != GameConst.AnimationParam.eState.Fall)
        motionState = GameConst.AnimationParam.eState.Fall;
    }
    public void Sit()
    {
        if (motionState != GameConst.AnimationParam.eState.Sit)
        motionState = GameConst.AnimationParam.eState.Sit;
    }
    public void Roll()
    {
        if (motionState != GameConst.AnimationParam.eState.Roll)
            motionState = GameConst.AnimationParam.eState.Roll;
    }
    public void Attack()
    {
        if (comboAvailableTime > Time.time)
            comboCount = ++comboCount % 4;
        else
        {
            comboCount = 0;
            comboAvailableTime = Time.time + comboAvailableInterval;
        }

        motionState = GameConst.AnimationParam.eState.Attack;
    }
    #endregion

    #region Callback
    private void OnStateChanged()
    {
        switch (this.motionState)
        {
            case GameConst.AnimationParam.eState.Die:
                animator.SetTrigger(GameConst.AnimationParam.die);
                break;
            case GameConst.AnimationParam.eState.Fall:
                animator.SetTrigger(GameConst.AnimationParam.fall);
                break;
            case GameConst.AnimationParam.eState.Idle:
                animator.SetTrigger(GameConst.AnimationParam.idle);
                break;
            case GameConst.AnimationParam.eState.Jump:
                animator.SetTrigger(GameConst.AnimationParam.jump);
                break;
            case GameConst.AnimationParam.eState.Attack:
                animator.SetTrigger(GameConst.AnimationParam.attack);
                animator.SetInteger(GameConst.AnimationParam.comboCount, comboCount);
                break;
            case GameConst.AnimationParam.eState.Roll:
                animator.SetTrigger(GameConst.AnimationParam.roll);
                break;
            case GameConst.AnimationParam.eState.Run:
                animator.SetTrigger(GameConst.AnimationParam.run);
                break;
            case GameConst.AnimationParam.eState.Sit:
                animator.SetTrigger(GameConst.AnimationParam.sit);
                break;
            default: break;
        }
    }
    #endregion
}
