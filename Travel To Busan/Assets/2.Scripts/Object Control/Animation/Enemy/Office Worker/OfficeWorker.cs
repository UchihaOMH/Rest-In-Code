using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct _OfficeWorkerAnimStates_
{
    public OfficeWorkerAttackState attack;
    public OfficeWorkerPatrolState patrol;
    public OfficeWorkerTraceState trace;
    public OfficeWorkerBeAttackedState beAttacked;
}
public struct _OfficeWorkerAnimTrigger_
{
    public const string idle = "Idle";
    public const string patrol = "Patrol";
    public const string trace = "Trace";
    public const string die = "Die";
    public const string attack = "Attack";
    public const string beAttacked = "Be Attacked";
}

public class OfficeWorker : Enemy, IManagedObject
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

    public Transform Pool
    {
        get => pool;
    }
    private Transform pool;
    #endregion

    #region Public Field
    [Header("Office worker Info"), Space(10f)]
    public _OfficeWorkerAnimStates_ animationState;

    [Header("Reference"), Space(10f)]
    public HPBar hpBar;
    public Transform tr;
    public Rigidbody2D rb;
    public AnyPortrait.apPortrait apPortrait;
    public AudioSource audioSource;

    [Header("Parameter"), Space(10f)]
    public Entity target;
    public float patrolSpeed = 0.15f;
    public float knockBackDist = 0.3f;
    #endregion

    #region Private Field
    [SerializeField] private bool isDebug = true;
    #endregion

    #region Mono
    private void Awake()
    {
        pool = GameObject.FindGameObjectWithTag("Enemy Pool").transform;
        wanderOrigin = new Vector2(tr.position.x, tr.position.y);
        CurrState = animationState.patrol;
    }
    private void Update()
    {
        if (info.currHP <= 0f)
        {
            OnDeadEvent();
            return;
        }

        if (target?.isDead == true)
            target = null;

        (CurrState as IAnimState).Process();
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
            target = _attacker;

            info.currHP -= GameManager.Instance.damageCalculator.CalcFinalDamage(_damage, info.defense);

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
        CurrState = _state as OfficeWorkerState;
        (CurrState as IAnimState).Process();
    }
    public override void FocusTarget(Entity _target)
    {
        target = _target;
        LookAt(_target.transform.position.x - tr.position.x < 0f ? Vector2.left : Vector2.right);
        apPortrait.CrossFade(_OfficeWorkerAnimTrigger_.trace);
        TransitionProcess(animationState.trace);
    }
    public override void OnDeadEvent()
    {
        isDead = true;

        hpBar.HideBar(true);
        
        //  충돌 및, 중력 옵션 해제
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<BoxCollider2D>().isTrigger = true;

        apPortrait.CrossFade(_OfficeWorkerAnimTrigger_.die);

        Invoke("ReturnObject2Pool", 3f);
    }
    #endregion

    #region IManagedObejct Method Override
    public void ReturnObject2Pool()
    {
        ResetObjectForPooling();
        tr.SetParent(Pool);
        gameObject.SetActive(false);
    }
    public void ResetObjectForPooling()
    {
        target = null;
        info.currHP = info.maxHP;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().isTrigger = false;

        apPortrait.Play(_OfficeWorkerAnimTrigger_.idle);
    }
    #endregion

    #region Animation Event
    private void OnAttackEvent(bool _attack)
    {
        animationState.attack.OnAttack(_attack);
    }
    private void OnAttackExitEvent()
    {
        animationState.attack.OnAttackExit();
    }
    #endregion
}
