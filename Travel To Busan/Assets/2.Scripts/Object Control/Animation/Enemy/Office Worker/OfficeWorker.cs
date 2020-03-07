using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyPortrait;

[System.Serializable]
public struct _OfficeWorkerAnimStates_
{
    public OfficeWorkerAttackState attack;
    public OfficeWorkerTraceState trace;
    public OfficeWorkerIdleState idle;
    public OfficeWorkerBeAttackedState beAttacked;
}
public struct _OfficeWorkerAnimTrigger_
{
    public const string idle = "Idle";
    public const string trace = "Trace";
    public const string die = "Die";
    public const string attack = "Attack";
    public const string beAttacked = "Be Attacked";
}

public class OfficeWorker : Enemy
{
    #region Property
    public OfficeWorkerState CurrState
    {
        get => currState;
        set
        {
            var before = currState;
            currState = value;
            if (isDebug && before != currState)
                Debug.Log("Office Worker State : " + (currState as IAnimState).GetStateName());
        }
    }
    [SerializeField] private OfficeWorkerState currState;

    public Entity Target
    {
        get => target;
        private set => target = value;
    }
    public override Transform Pool
    {
        get => pool;
        set => pool = value;
    }
    #endregion

    #region Public Field
    [Header("Office worker Info"), Space(10f)]
    public _OfficeWorkerAnimStates_ animationState;

    [Header("Reference"), Space(10f)]
    public HPBar hpBar;
    public Transform tr;
    public Rigidbody2D rb;
    public apPortrait apPortrait;
    public AudioSource audioSource;
    #endregion

    #region Private Field
    [SerializeField] private bool isDebug = false;
    #endregion

    #region Mono
    private void OnEnable()
    {
        Pool = GameObject.FindGameObjectWithTag("Enemy Pool").transform;
        hpBar.HideBar(true);
        CurrState = animationState.idle;
    }
    private void Update()
    {
        if (info.currHP <= 0f)
        {
            OnDeadEvent();
            return;
        }

        if (!isDead)
        {
            if (Target?.isDead == true)
            {
                Target = null;
                apPortrait.Play(_OfficeWorkerAnimTrigger_.idle);
                TransitionProcess(animationState.idle);
            }

            (CurrState as IAnimState).Process();
        }
    }
    private void LateUpdate()
    {
        hpBar.FillAmount(info.currHP / info.maxHP);
    }
    #endregion

    #region Entity Method Override
    public override void BeAttacked(Entity _attacker, float _damage, Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.3f)
    {
        if (_attacker is Player)
        {
            if (CurrState == animationState.attack)
                OnAttackExitEvent();

            Target = _attacker;

            info.currHP -= GameManager.Instance.DamageCalculator.CalcFinalDamage(_damage, info.defense);

            TransitionProcess(animationState.beAttacked);
            (CurrState as OfficeWorkerBeAttackedState).BeAttacked(_knockBackDir, _knockBackDist, _knockBackDuration);
        }
    }
    public override void LookAt(Vector2 _dir)
    {
        if (_dir == Vector2.left)
        {
            tr.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (_dir == Vector2.right)
        {
            tr.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
            return;
    }
    public override void TransitionProcess(IAnimState _state)
    {
        if (!isDead)
        {
            CurrState = _state as OfficeWorkerState;
            (CurrState as IAnimState).Process();
        }
    }
    public override void FocusTarget(Entity _target)
    {
        Target = _target;
        LookAt(_target.transform.position.x - tr.position.x < 0f ? Vector2.left : Vector2.right);
        apPortrait.CrossFade(_OfficeWorkerAnimTrigger_.trace);
        TransitionProcess(animationState.trace);
    }
    public override void OnDeadEvent()
    {
        if (!isDead)
        {
            isDead = true;

            hpBar.HideBar(true);

            //  충돌 및, 중력 옵션 해제
            rb.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<BoxCollider2D>().isTrigger = true;

            apPortrait.Play(_OfficeWorkerAnimTrigger_.die);

            Invoke("ReturnObject2Pool", 3f);
        }
    }
    #endregion

    #region IManagedObejct Method Override
    public override void ReturnObject2Pool()
    {
        ResetObjectForPooling();
        tr.SetParent(Pool);
        gameObject.SetActive(false);
    }
    public override void ResetObjectForPooling()
    {
        apPortrait.Play(_OfficeWorkerAnimTrigger_.idle);
        isDead = false;
        Target = null;
        info.currHP = info.maxHP;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
    #endregion

    #region Animation Event
    private void OnAttackEvent()
    {
        animationState.attack.OnAttack();
    }
    private void OnAttackExitEvent()
    {
        animationState.attack.OnAttackExit();
    }
    #endregion
}
