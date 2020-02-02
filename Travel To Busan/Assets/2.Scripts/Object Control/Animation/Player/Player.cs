using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct _PlayerAnimTrigger_
{
    public const string bJump = "bJump";
    public const string bSit = "bSit";

    public const string fRunBlend = "fRunBlend";
    public const string fJumpBlend = "fJumpBlend";

    public const string tRoll = "tRoll";
    public const string tBeAttacked = "tBeAttacked";
    public const string tBaseAttack = "tBaseAttack";
    public const string tJumpAttack = "tJumpAttack";
    public const string tDie = "tDie";
}
[System.Serializable]
public struct _PlayerAnimState_
{
    public PlayerRunState run;
    public PlayerJumpState jump;
    public PlayerRollState roll;
    public PlayerSitState sit;
    public PlayerDieState die;
    public PlayerBaseAttackState baseAttack;
    public PlayerJumpAttackState jumpAttack;
    public PlayerBeAttackedState beAttacked;
    public PlayerCmdState cmd;
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
                Debug.Log((currState as IAnimState).GetStateName());
        }
    }
    private PlayerState currState;
    /// <summary>
    /// Direction of the Control Pad, player touched
    /// </summary>
    public string CurrentDirection
    {
        get => currentDirection;
        private set => currentDirection = value;
    }
    private string currentDirection = "";
    #endregion

    #region Public Field    
    [Header("Raycast")]
    public GraphicRaycaster raycaster;
    public PointerEventData eventData;

    [Header("Controller"), Space(10f)]
    public HPBar hpBar;
    public RectTransform controlPad;
    public RectTransform controlStick;
    public GameObject attackButton;
    public GameObject rollButton;
    public GameObject cmdButton;

    [Header("Target Control"), Space(10f)]
    public Transform targetHandTr;
    public Transform tr;
    public Animator anim;
    public Rigidbody2D rb;
    public Puppet2D.Puppet2D_GlobalControl globalControl;
    
    [Space(10f)]
    public Weapon weapon;
    public float jumpForce;
    public float rollSpeed;
    public _PlayerAnimState_ animationStates;
    public bool onGround = true;
    #endregion

    #region Private Field
    private Vector3 controlPadCenterPos;
    private GameObject controlpadCompass;
    private float controlPadRadius = 0f;
    private int controlPadTouchId = -1;

    [SerializeField] private bool isDebug = true;
    #endregion

    #region Mono
    private void Awake()
    {
        gameObject.layer = LayerMask.GetMask(GameConst.LayerDefinition.player);

        hpBar.FillAmount(info.currHP / info.maxHP);

        eventData = new PointerEventData(EventSystem.current);
        CurrState = animationStates.run;

        controlpadCompass = new GameObject("CompassObj");
        controlpadCompass.transform.position = controlPad.position;
        controlpadCompass.transform.SetParent(controlPad);
        controlPadCenterPos = controlPad.position;
        controlPadRadius = controlPad.rect.size.x / 2f;
    }
    private void Update()
    {
        JumpCheck();
        CurrentDirection = !GetControlPadInput().Equals(GameConst.emptyKeyValuePair) ? GetControlPadInput().Key : "";
        (CurrState as IAnimState).Process();

        if (isDebug)
        {
            //Debug.Log(CurrentDirection);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Collider2D>() != null)
        {
            if (collision.GetContact(0).normal.y > 0f)
            {
                onGround = true;

                if (CurrState is PlayerJumpState)
                {
                    CurrState = animationStates.run;
                    anim.SetBool("bJump", false);
                }
            }
        }
        //if (LayerMask.LayerToName(collision.collider.gameObject.layer) == GameConst.LayerDefinition.level)
        //{
        //    if (collision.GetContact(0).normal.y > 0f)
        //    {
        //        onGround = true;

        //        if (CurrState is PlayerJumpState)
        //        {
        //            CurrState = animationStates.run;
        //            anim.SetBool("bJump", false);
        //        }
        //    }
        //}
    }
    #endregion

    #region Public Method
    /// <summary>
    /// Return the control pad input
    /// </summary>
    /// <returns>Key : The direction of the control stick from the control pad, Value : The touch phase from the control pad</returns>
    public KeyValuePair<string, TouchPhase> GetControlPadInput()
    {
        if (Input.touchCount == 0)
            HoldControlStick(controlPadCenterPos);

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            List<RaycastResult> set = new List<RaycastResult>();
            eventData.position = touch.position;
            raycaster.Raycast(eventData, set);

            if (controlPadTouchId == touch.fingerId || (set.Count > 0 && set[0].gameObject == controlPad.gameObject))
            {
                HoldControlStick(touch.position);
                controlPadTouchId = touch.fingerId;

                string direction = CurrentDirection = ParseStringDirection(controlStick.position);
                KeyValuePair<string, TouchPhase> returnValue = new KeyValuePair<string, TouchPhase>(direction, touch.phase);

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    HoldControlStick(controlPadCenterPos);
                    controlPadTouchId = -1;
                }

                return returnValue;
            }
            else
                continue;
        }

        return GameConst.emptyKeyValuePair;
    }
    public void EquipWeapon(GameObject _weapon)
    {
        if (weapon != null)
            Destroy(weapon.gameObject);

        weapon = _weapon.GetComponent<Weapon>();
        weapon.transform.SetParent(targetHandTr);
        weapon.transform.position = Vector3.zero;

        weapon.transform.localPosition = weapon.weaponOffset.position;
        weapon.transform.localRotation = Quaternion.Euler(weapon.weaponOffset.rotation);
        weapon.transform.localScale = weapon.weaponOffset.scale;

        weapon.owner = this;
    }
    #endregion

    #region Private Mathod
    /// <summary>
    /// Does player jump?
    /// </summary>
    /// <returns>true : Player jump</returns>
    private void JumpCheck()
    {
        if (rb.velocity.y < -0.001f || rb.velocity.y > 0.001f)
        {
            onGround = false;

            if (!(CurrState is PlayerBaseAttackState) && !(CurrState is PlayerRollState) && !(CurrState is PlayerCmdState) && !(CurrState is PlayerJumpAttackState))
            {
                CurrState = animationStates.jump;
                anim.SetBool("bJump", true);
                anim.SetFloat("fJumpBlend", rb.velocity.y);
            }
        }
    }
    /// <summary>
    /// The direction of the control stick from the control pad
    /// </summary>
    /// <param name="_pos">Typically, touched position</param>
    /// <returns>The Direction string. examply "Left"</returns>
    private string ParseStringDirection(Vector2 _pos)
    {
        Vector2 dir = _pos - (new Vector2(controlPad.position.x, controlPad.position.y));
        float rot = Vector2.SignedAngle(Vector2.up, dir);

        if (Mathf.Abs(rot) < 20f)
        {
            return "Up";
        }
        else if (Mathf.Abs(rot) < 50f)
        {
            if (rot < 0f)
            {
                return "Up Right";
            }
            else
            {
                return "Up Left";
            }
        }
        else if (Mathf.Abs(rot) < 160f)
        {
            if (rot < 0f)
            {
                return "Right";
            }
            else
            {
                return "Left";
            }
        }
        else if (Mathf.Abs(rot) <= 180f)
        {
            return "Down";
        }

        return "";
    }
    /// <summary>
    /// position the control stick where specify position
    /// </summary>
    /// <param name="_pos">Typically, touched position</param>
    private void HoldControlStick(Vector3 _pos)
    {
        float distance = Vector3.Distance(controlPadCenterPos, _pos);
        if (distance <= controlPadRadius)
        {
            controlStick.position = _pos;
        }
        else
        {
            Vector3 newPos = (_pos - controlPadCenterPos).normalized * controlPadRadius;
            controlStick.position = controlPadCenterPos + newPos;
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
            globalControl.flip = false;
        }
        else if (dir.x > 0.1f)
        {
            globalControl.flip = true;
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
    public override void BeAttacked(float _damage)
    {
        float finalDamage = GameManager.Instance.damageCalculator.CalcFinalDamage(_damage, info.defense);
        info.currHP -= finalDamage;
        hpBar.FillAmount(info.currHP / info.maxHP);
    }
    public override void KnockBack(Vector2 _dir, float _power)
    {
        rb.AddForce(_dir.normalized * _power, ForceMode2D.Impulse);
        TransitionProcess(animationStates.beAttacked);
    }

    public override void OnDeadEvent()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
