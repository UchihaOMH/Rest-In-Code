using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyPortrait;

public class UnderArmourCollector : Enemy
{
    [System.Serializable]
    public struct _UnderArmourCollectorState_
    {
        public UnderArmourCollectorIdleState idle;
        public UnderArmourCollectorPatternBridgeState patternBridge;
        public UnderArmourCollectorAttackState attack;
        public UnderArmourCollectorSmashState smash;
        public UnderArmourCollectorRushState rush;
        public UnderArmourCollectorThrowProteinState throwAttack;
    }
    [System.Serializable]
    public struct _UnderArmourCollectorAnim_
    {
        public static string idle = "Idle";
        public static string run = "Run";
        public static string attack = "Attack";
        public static string smash = "Smash";
        public static string rush = "Rush";
        public static string throwProtein = "Throw Protein";
        public static string die = "Die";
    }

    #region Property
    public bool OnDamaged
    {
        get => onDamaged;
        private set => onDamaged = value;
    }
    public UnderArmourCollectorState CurrState
    {
        get => currState;
        set => currState = value;
    }
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
    public _UnderArmourCollectorState_ animState;

    [Header("Component Reference"), Space(15f)]
    public apPortrait apPortrait;
    public Animator fxAnimator;
    #endregion

    #region Private Field
    private bool onDamaged = false;
    [SerializeField] private UnderArmourCollectorState currState;
    #endregion

    private void OnEnable()
    {
        Pool = GameObject.FindGameObjectWithTag("Enemy Pool").transform;

        CurrState = animState.idle;
    }
    private void Update()
    {
        if (info.currHP <= 0f)
            OnDeadEvent();

        if (!isDead)
        {
            (CurrState as IAnimState).Process();
        }
    }

    #region Enemy Override
    public override void BeAttacked(Entity _attacker, float _damage, Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.3F)
    {
        info.currHP -= GameManager.Instance.DamageCalculator.CalcFinalDamage(_damage, info.defense);
        FocusTarget(_attacker);
    }
    public override void FocusTarget(Entity _target)
    {
        if (Target == null)
        {
            Target = _target;
            TransitionProcess(animState.patternBridge);
            animState.patternBridge.timer = Time.time + 1f;
        }
        else
            Target = _target;
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
            apPortrait.Play(_UnderArmourCollectorAnim_.die);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().isTrigger = true;
            apPortrait.Play(_UnderArmourCollectorAnim_.die);
            fxAnimator.Play("Bridge");

            Invoke("ReturnObject2Pool", 3f);
        }
    }
    public override void TransitionProcess(IAnimState _state)
    {
        CurrState = _state as UnderArmourCollectorState;
    }
    #endregion

    #region Animation Event
    private void OnAttack()
    {
        if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.attack))
            animState.attack.OnAttack();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.smash))
            animState.smash.OnAttack();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.rush))
            animState.rush.OnAttack();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.throwProtein))
            animState.throwAttack.OnAttack();
    }
    private void OnAttackEnter()
    {
        if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.attack))
            animState.attack.OnAttackEnter();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.smash))
            animState.smash.OnAttackEnter();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.rush))
            animState.rush.OnAttackEnter();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.throwProtein))
            animState.throwAttack.OnAttackEnter();
    }
    private void OnAttackExit()
    {
        if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.attack))
            animState.attack.OnAttackExit();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.smash))
            animState.smash.OnAttackExit();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.rush))
            animState.rush.OnAttackExit();
        else if (apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.throwProtein))
            animState.throwAttack.OnAttackExit();
    }
    #endregion

    #region Managed Object Override
    public override void ResetObjectForPooling()
    {
        apPortrait.Play(_UnderArmourCollectorAnim_.idle);
        isDead = false;
        info.currHP = info.maxHP;
        target = null;

        gameObject.SetActive(false);
    }

    public override void ReturnObject2Pool()
    {
        ResetObjectForPooling();
        transform.SetParent(Pool);
    }
    #endregion
}
