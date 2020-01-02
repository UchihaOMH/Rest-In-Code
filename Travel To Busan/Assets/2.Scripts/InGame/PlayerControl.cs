using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct AnimStateStruct
{
    public PlayerAnimState run;
    public PlayerAnimState jump;
    public PlayerAnimState roll;
    public PlayerAnimState sit;
    public PlayerAnimState die;
    public PlayerAnimState attack;

    public AnimStateStruct(PlayerAnimState run, PlayerAnimState jump, PlayerAnimState roll, PlayerAnimState sit, PlayerAnimState die, PlayerAnimState attack)
    {
        this.run = run;
        this.jump = jump;
        this.roll = roll;
        this.sit = sit;
        this.die = die;
        this.attack = attack;
    }
}

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance
    {
        get => instance;
        private set => instance = value;
    }
    public PlayerAnimState State
    {
        get => state;
        set
        {
            state = value;
            state.CurrentState();
        }
    }

    #region Public Field    
    [Header("Raycast")]
    public GraphicRaycaster raycaster;
    public PointerEventData eventData;

    [Header("Controller"), Space(10f)]
    public RectTransform controllerRect;
    public GameObject attackSymbol;
    public GameObject upButton;
    public GameObject leftButton;
    public GameObject downButton;
    public GameObject rightButton;
    public GameObject upLeftButton;
    public GameObject upRightButton;
    public GameObject downLeftButton;
    public GameObject downRightButton;

    [Header("Target Control"), Space(10f)]
    public GameObject target;
    public Animator targetAnim;
    public Transform targetTr;
    public Rigidbody2D targetRb;
    public SpriteRenderer targetRender;
    public BoxCollider2D targetCld;

    public float speed = 0.3f;
    public float jumpForce = 30f;
    public float rollDistance = 2.0f;

    public bool doesSpriteLookingRight = false;
    public bool isDead = false;

    /// <summary>
    /// default : -1
    /// </summary>
    //public int currTouchIndex = -1;

    [Header("Animation Control"), Space(10f)]
    public AnimStateStruct animStruct;
    #endregion

    #region Private Field
    private static PlayerControl instance;
    private PlayerAnimState state;
    #endregion

    #region Mono
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
            Destroy(this.gameObject);

        eventData = new PointerEventData(EventSystem.current);
        animStruct = new AnimStateStruct(new PlayerRunState(), new PlayerJumpState(), new PlayerRollState(), new PlayerSitState(), new PlayerDieState(), new PlayerAttackState());
        State = animStruct.run;
    }
    private void Update()
    {
        Touch touch = new Touch();
        GameObject obj = null;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch currTouch = Input.GetTouch(i);
            List<RaycastResult> set = new List<RaycastResult>();
            eventData.position = currTouch.position;
            raycaster.Raycast(eventData, set);

            if (set.Count > 0)
            {
                if (LayerMask.LayerToName(set[0].gameObject.layer) == GameConst.LayerDefinition.controller)
                {
                    obj = set[0].gameObject;
                    touch = currTouch;
                }
            }
        }

        State.InputProcess(obj, touch);
    }
    private void OnGUI()
    {
        //if (GUI.Button(Rect.MinMaxRect(100f, 100f, Screen.width / 2, Screen.height / 5), "머리아퍼"))
        //{
        //    
        //}
    }
    #endregion

    #region Method
    public void LookAt(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return;

        if (dir.x < -0.1f)
        {
            if (doesSpriteLookingRight)
                targetTr.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            else
                targetTr.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if (dir.x > 0.1f)
        {
            if (doesSpriteLookingRight)
                targetTr.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            else
                targetTr.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }
    public bool JumpCheck()
    {
        if (targetRb.velocity.y < -0.1f)
        {
            return true;
        }
        else if (targetRb.velocity.y > 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
