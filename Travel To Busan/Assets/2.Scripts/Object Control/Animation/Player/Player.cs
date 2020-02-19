using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public struct _PlayerAnimTrigger_
{
    public const string idle = "Idle";
    public const string run = "Run";
    public const string jump = "Jump";
    public const string fall = "Fall";
    public const string die = "Die";
    public const string beAttacked = "Be Attacked";

    public const string attack = "Attack";
    public const string smashAttack = "Smash Attack";
    public const string rizingAttack = "Rizing Attack";
}
[System.Serializable]
public struct _PlayerAnimState_
{
    public PlayerRunState run;
    public PlayerJumpState jump;
    public PlayerAttackState attack;
    public PlayerBeAttackedState beAttacked;
    public PlayerSkillState skill;
}
[System.Serializable]
public struct _PlayerInfo_
{
    public string name;

    public float damage;
    public float defense;

    public float maxHP;
    public float currHP;

    public float speed;
}

public class Player : Entity
{
    #region Property
    public PlayerState CurrState
    {
        get => currState;
        set
        {
            var before = currState;
            currState = value;
            if (isDebug && before != currState)
                Debug.Log("Player State : " + (currState as IAnimState).GetStateName());
        }
    }
    #endregion

    #region Public Field    
    [Header("Controller"), Space(15f)]
    public HPBar hpBar;
    public ControlPadInputModule inputModule;

    [Header("Target Control"), Space(15f)]
    public AnyPortrait.apPortrait apPortrait;
    public Transform targetHandTr;
    public Transform tr;
    public Rigidbody2D rb;
    public AudioSource audioSource;

    [Space(15f)]
    public Weapon weapon;
    public _PlayerAnimState_ animationStates;
    public float jumpForce;
    public bool onGround = true;
    #endregion

    #region Private Field
    [SerializeField, Header("Debug")] private PlayerState currState;
    [SerializeField] bool isCinematicMode = false;
    [SerializeField] private bool isDebug = true;
    #endregion

    #region Mono
    private void Awake()
    {
        CurrState = animationStates.run;

        //  시네마틱에서는 불필요한 초기화를 안함
        if (isCinematicMode)
            return;

        hpBar.FillAmount(info.currHP / info.maxHP);
        hpBar.HideBar(false);
    }
    private void Update()
    {
        if (info.currHP <= 0f)
        {
            OnDeadEvent();
            return;
        }

        JumpCheck();

        (CurrState as IAnimState).Process();

        if (isDebug)
        {
            //Debug.Log(CurrentDirection);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Collider2D>() != null && !onGround)
        {
            if (collision.GetContact(0).normal.y > 0f)
            {
                onGround = true;

                if (CurrState is PlayerJumpState)
                {
                    CurrState = animationStates.run;
                    apPortrait.Play(_PlayerAnimTrigger_.idle);
                }
            }
        }
    }
    #endregion

    #region Public Method
    public void EquipWeapon(GameObject _weaponObj)
    {
        if (weapon != null)
            Destroy(weapon.gameObject);

        Weapon newWeapon = _weaponObj.GetComponent<Weapon>();
        weapon = newWeapon;

        weapon.owner = this;
        weapon.transform.SetParent(targetHandTr);
        weapon.transform.position = weapon.weaponOffset.position;
        weapon.transform.rotation = Quaternion.Euler(weapon.weaponOffset.rotation);
        weapon.transform.localScale = weapon.weaponOffset.scale;
    }
    #endregion

    #region Private Method
    private void JumpCheck()
    {
        if (rb.velocity.y > 0.001f)
        {
            onGround = false;

            if (!(CurrState is PlayerAttackState) && !(CurrState is PlayerSkillState))
            {
                CurrState = animationStates.jump;
                if (!apPortrait.IsPlaying(_PlayerAnimTrigger_.jump))
                    apPortrait.Play(_PlayerAnimTrigger_.jump);
            }
        }
        else if (rb.velocity.y < -0.001f)
        {
            onGround = false;

            if (!(CurrState is PlayerAttackState) && !(CurrState is PlayerSkillState))
            {
                CurrState = animationStates.jump;
                if (!apPortrait.IsPlaying(_PlayerAnimTrigger_.fall))
                    apPortrait.Play(_PlayerAnimTrigger_.fall);
            }
        }
    }
    #endregion

    #region Entity Override Method
    /// <summary>
    /// Flip sprite
    /// </summary>
    /// <param name="dir">Left or Right</param>
    public override void LookAt(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return;

        if (dir.x < -0.1f)
        {
            tr.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (dir.x > 0.1f)
        {
            tr.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    /// <summary>
    /// Change state
    /// </summary>
    /// <param name="state">Target state</param>
    public override void TransitionProcess(IAnimState state)
    {
        CurrState = state as PlayerState;
        (CurrState as IAnimState).Process();
    }
    public override void BeAttacked(Entity _attacker, float _damage, Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.3f)
    {
        float finalDamage = GameManager.Instance.damageCalculator.CalcFinalDamage(_damage, info.defense);
        info.currHP -= finalDamage;
        hpBar?.FillAmount(info.currHP / info.maxHP);
        TransitionProcess(animationStates.beAttacked);
        (CurrState as PlayerBeAttackedState).BeAttacked(_knockBackDir, _knockBackDist, _knockBackDuration);

        apPortrait.Play(_PlayerAnimTrigger_.beAttacked);
    }
    public override void OnDeadEvent()
    {
        if (!isDead)
        {
            isDead = true;
            apPortrait.Play(_PlayerAnimTrigger_.die);
        }
    }
    #endregion

    #region Animation Event Method
    private void OnAttackExit()
    {
        if (apPortrait.IsPlaying(_PlayerAnimTrigger_.attack))
            animationStates.attack.OnAttackExit();
        else if (apPortrait.IsPlaying(_PlayerAnimTrigger_.rizingAttack))
            animationStates.skill.OnRizingAttackExit();
        else if (apPortrait.IsPlaying(_PlayerAnimTrigger_.smashAttack))
            animationStates.skill.OnSmashExit();
    }
    private void OnAttack()
    {
        if (apPortrait.IsPlaying(_PlayerAnimTrigger_.rizingAttack))
            animationStates.skill.OnRizingAttack();
    }
    #endregion
}
