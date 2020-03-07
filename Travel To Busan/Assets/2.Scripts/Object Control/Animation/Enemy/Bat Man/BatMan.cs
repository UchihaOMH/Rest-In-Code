using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyPortrait;

public class BatMan : Enemy 
{
    [System.Serializable]
    public struct _BatManAnim_
    {
        public static string idle = "Idle";
        public static string walk = "Walk";
        public static string run = "Run";
        public static string die = "Die";
        public static string attack = "Attack";
        public static string swing = "Swing";
        public static string shoutBall = "Shout Ball";
    }
    [System.Serializable]
    public struct _BatManState_
    {
        public BatManPatternBridgeState patternBridgeState;
        public BatManAttackState attackState;
        public BatManSwingState swingState;
        public BatManShoutBallState shoutBallState;
    }

    #region Property
    public override Transform Pool
    {
        get => pool;
        set => pool = value;
    }
    public BatManState CurrState
    {
        get => currState;
        set => currState = value;
    }
    public Entity Target
    {
        get => target;
        set => target = value;
    }
    #endregion

    #region Public Field
    [Header("Component Reference"), Space(15f)]
    public apPortrait apPortrait;
    public Animator fxAnimator;

    [Header("Parameter"), Space(15f)]
    public _BatManState_ animState;
    #endregion

    #region Private Field
    [SerializeField] private BatManState currState;
    #endregion

    private void OnEnable()
    {
        Pool = GameObject.FindGameObjectWithTag("Enemy Pool").transform;

        apPortrait.Initialize();

        if (!apPortrait.IsPlaying(_BatManAnim_.idle))
            apPortrait.Play(_BatManAnim_.idle);

        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (Target != null)
            CurrState = animState.patternBridgeState;
    }
    private void Update()
    {
        if (info.currHP <= 0f)
            OnDeadEvent();

        if (!isDead)
        {
            if (Target.isDead)
            {
                currState = null;
                apPortrait.Play(_BatManAnim_.idle);
            }

            if (currState == null)
                return;

            currState.Process();
        }
    }

    #region Enemy Override
    public override void BeAttacked(Entity _attacker, float _damage, Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.3F)
    {
        info.currHP -= GameManager.Instance.DamageCalculator.CalcFinalDamage(_damage, info.defense);
        if (Target == null)
            FocusTarget(_attacker);
    }
    public override void FocusTarget(Entity _target)
    {
        Target = _target;
        TransitionProcess(animState.patternBridgeState);
    }
    public override void LookAt(Vector2 _dir)
    {
        if (_dir == Vector2.left)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (_dir == Vector2.right)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    public override void OnDeadEvent()
    {
        if (!isDead)
        {
            isDead = true;
            apPortrait.Play(_BatManAnim_.die);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().isTrigger = true;

            Invoke("ReturnObject2Pool", 3f);
        }
    }
    public override void TransitionProcess(IAnimState _state)
    {
        CurrState = _state as BatManState;
    }
    #endregion

    #region Managed Object Override
    public override void ResetObjectForPooling()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().isTrigger = false;

        Target = null;
        currState = null;
        isDead = false;

        info.currHP = info.maxHP;
    }
    public override void ReturnObject2Pool()
    {
        ResetObjectForPooling();
        transform.SetParent(Pool);
        gameObject.SetActive(false);
    }
    #endregion


    #region Animation Event
    public void OnAttackEnter()
    {
        if (apPortrait.IsPlaying(_BatManAnim_.attack))
            animState.attackState.OnAttackEnter();
        else if (apPortrait.IsPlaying(_BatManAnim_.swing))
            animState.swingState.OnAttackEnter();
        else if (apPortrait.IsPlaying(_BatManAnim_.shoutBall))
            animState.shoutBallState.OnAttackEnter();
    }
    public void OnAttack()
    {
        if (apPortrait.IsPlaying(_BatManAnim_.attack))
            animState.attackState.OnAttack();
        else if (apPortrait.IsPlaying(_BatManAnim_.swing))
            animState.swingState.OnAttack();
        else if (apPortrait.IsPlaying(_BatManAnim_.shoutBall))
            animState.shoutBallState.OnAttack();
    }
    public void OnAttackExit()
    {
        if (apPortrait.IsPlaying(_BatManAnim_.attack))
            animState.attackState.OnAttackExit();
        else if (apPortrait.IsPlaying(_BatManAnim_.swing))
            animState.swingState.OnAttackExit();
        else if (apPortrait.IsPlaying(_BatManAnim_.shoutBall))
            animState.shoutBallState.OnAttackExit();
    }
    #endregion
}
