using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerState : MonoBehaviour
{
    #region Public Fields
    public Transform tr;
    public Animator animator;
    public SpriteRenderer render;

    public float moveSpeed;
    public float jumpHeight;
    public float rollDist;
    #endregion

    #region Public Property
    public float maxHp
    {
        get => _maxHp;
    }
    public float maxEnergy
    {
        get => _maxEnergy;
    }
    public float hp
    {
        get => _hp;
    }
    public float energy
    {
        get => _energy;
    }
    public int level
    {
        get => _level;
    }
    public float exp
    {
        get => _exp;
    }
    public GameConst.AnimationParam.eAnim state
    {
        get => _state;
        set
        {
            previousState = _state;
            _state = value;
            OnStateChanged();
        }
    }
    public bool isControllerLocked
    {
        get => _isControllerLocked;
        private set => _isControllerLocked = value;
    }
    public bool isJump
    {
        get => _isJump;
        private set => _isJump = value;
    }
    /// <summary>
    /// The direction that sprite is looking at
    /// </summary>
    public GameConst.SpriteDefaultDirection spriteDefaultDirection;
    #endregion

    #region Private Fields
    private float _maxHp;
    private float _maxEnergy;
    private float _hp;
    private float _energy;
    private int _level;
    private float _exp;
    
    private bool _isControllerLocked = false;
    private bool _isJump = false;
    private int comboCount = 0;
    private int maxComboCount = 3;

    private GameConst.AnimationParam.eAnim _state;
    private GameConst.AnimationParam.eAnim previousState;
    #endregion

    #region Mono Methods
    #endregion

    #region Method
    public void Run(Vector3 dir)
    {
        if (isControllerLocked)
            return;

        state = GameConst.AnimationParam.eAnim.Run;

        if (dir.x > 0)
        {
            tr.Translate(new Vector3(moveSpeed, 0, 0));
            if (spriteDefaultDirection.Equals(GameConst.SpriteDefaultDirection.Left))
                render.flipX = true;
            else
                render.flipX = false;
        }
        else
        {
            tr.Translate(new Vector3(-moveSpeed, 0, 0));
            if (spriteDefaultDirection.Equals(GameConst.SpriteDefaultDirection.Left))
                render.flipX = false;
            else
                render.flipX = true;
        }
    }
    public void Idle()
    {
        state = GameConst.AnimationParam.eAnim.Idle;
    }
    public void Die()
    {
        isControllerLocked = true;
        state = GameConst.AnimationParam.eAnim.Die;
    }
    public void Jump()
    {
        StartCoroutine(JumpCoroutine());
    }
    public void Sit()
    {
        state = GameConst.AnimationParam.eAnim.Sit;
    }
    public void Roll(Vector3 dir)
    {
        state = GameConst.AnimationParam.eAnim.Roll;
        isControllerLocked = true;

        StartCoroutine(RollCoroutine(dir));

        isControllerLocked = false;
    }
    public void Kick()
    {
        state = GameConst.AnimationParam.eAnim.Kick;
    }
    public void Punch()
    {
        state = GameConst.AnimationParam.eAnim.Punch;
    }
    #endregion

    #region Callback
    /// <summary>
    /// If you change 'state' field, will this callback change the current animation to specified
    /// </summary>
    void OnStateChanged()
    {
        switch (this.state)
        {
            case GameConst.AnimationParam.eAnim.Run:
                animator.SetTrigger(GameConst.AnimationParam.tRun);
                break;
            case GameConst.AnimationParam.eAnim.Idle:
                animator.SetTrigger(GameConst.AnimationParam.tIdle);
                break;
            case GameConst.AnimationParam.eAnim.Die:
                animator.SetTrigger(GameConst.AnimationParam.tDie);
                break;
            case GameConst.AnimationParam.eAnim.Jump:
                animator.SetTrigger(GameConst.AnimationParam.tJump);
                break;
            case GameConst.AnimationParam.eAnim.Sit:
                animator.SetTrigger(GameConst.AnimationParam.tSit);
                break;
            case GameConst.AnimationParam.eAnim.Roll:
                animator.SetTrigger(GameConst.AnimationParam.tRoll);
                break;
            case GameConst.AnimationParam.eAnim.Kick:
                animator.SetTrigger(GameConst.AnimationParam.tKick);
                animator.SetInteger(GameConst.AnimationParam.iComboCount, ++comboCount);
                break;
            case GameConst.AnimationParam.eAnim.Punch:
                animator.SetTrigger(GameConst.AnimationParam.tPunch);
                animator.SetInteger(GameConst.AnimationParam.iComboCount, ++comboCount);
                break;

            default: break;
        }
    }
    #endregion

    #region Coroutine Method
    IEnumerator JumpCoroutine()
    {
        if (!(state == GameConst.AnimationParam.eAnim.Jump))
        {
            float targetPosY = tr.position.y + jumpHeight;

            while (tr.position.y < jumpHeight)
            {
                float dir = Mathf.Lerp(tr.position.y, targetPosY, Time.deltaTime);
                tr.Translate(new Vector3(0, dir, 0));

                yield return null;
            }

            state = GameConst.AnimationParam.eAnim.Jump;
        }
    }
    IEnumerator RollCoroutine(Vector3 dir)
    {
        if (!isControllerLocked)
        {
            isControllerLocked = true;
            state = GameConst.AnimationParam.eAnim.Roll;

            float startPos = tr.position.x;
            float destPos = startPos + rollDist;

            float currPos = startPos;
            while (currPos < destPos)
            {
                currPos = Mathf.Lerp(startPos, destPos, Time.deltaTime);
                tr.Translate(new Vector3(0, currPos, 0));

                yield return null;
            }

            isControllerLocked = false;
        }
    }
    #endregion
}
