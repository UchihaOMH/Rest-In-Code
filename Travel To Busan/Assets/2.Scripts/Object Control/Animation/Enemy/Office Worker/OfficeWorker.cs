using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct _OfficeWorkerAnimStates_
{
    public OfficeWorkerAttackState attack;
    public OfficeWorkerHowlState howl;
    public OfficeWorkerPatrolState patrol;
    public OfficeWorkerTraceState trace;
}
public struct _OfficeWorkerAnimTrigger_
{
    public const string fPatrolBlend = "fPatrolBlend";
    
    public const string bChase = "bChase";

    public const string tDie = "tDie";
    public const string tAttack = "tAttack";
    public const string tHowl = "tHowl";
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
            if (isDebug)
                Debug.Log((currState as IAnimState).GetStateName());
        }
    }
    private OfficeWorkerState currState;

    public Transform Pool
    {
        get => pool;
    }
    private Transform pool;
    #endregion

    #region Public Field
    [Header("Reference"), Space(10f)]
    public Transform tr;
    public Rigidbody2D rb;
    public Animator anim;
    public AudioSource audioSource;
    public Puppet2D.Puppet2D_GlobalControl globalControl;

    [Space(10f)]
    public _OfficeWorkerAnimStates_ animationState;
    public float patrolSpeed;
    #endregion

    #region Private Field
    [SerializeField] private bool isDebug = true;
    #endregion

    #region Mono
    private void Awake()
    {
        wanderOrigin = new Vector2(tr.position.x, tr.position.y);
        CurrState = animationState.patrol;
    }
    private void Update()
    {
        (CurrState as IAnimState).Process();
    }
    #endregion

    #region Entity Method Override
    public override void BeAttacked(float _damage)
    {
        throw new System.NotImplementedException();
    }
    public override void KnockBack(Vector2 _knockBackDir, float _knockBackPower)
    {
        throw new System.NotImplementedException();
    }
    public override void LookAt(Vector2 _dir)
    {
        if (_dir == Vector2.left)
        {
            globalControl.flip = false;
        }
        else if (_dir == Vector2.right)
        {
            globalControl.flip = true;
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
        animationState.trace.SetTarget(_target);
        TransitionProcess(animationState.trace);
    }
    public override void OnDeadEvent()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IManagedObejct Method Override
    public void ReturnObject2Pool()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
