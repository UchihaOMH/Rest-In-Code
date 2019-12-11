using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public IPlayerAnimState State
    {
        get => state;
        set
        {
            state = value;
            state.InputProcess();
        }
    }

    #region Public Field    
    [Header("Raycast")]
    public GraphicRaycaster graphicRaycaster;
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

    public float moveSpeed = 0.3f;
    public float jumpForce = 30f;
    public float rollDistance = 2.0f;

    public bool doesSpriteLookingRight = false;
    public bool isDead = false;

    [Header("Animation Control"), Space(10f)]
    public PlayerMoveState move;
    public PlayerJumpState jump;
    public PlayerRollState roll;
    public PlayerSitState sit;
    public PlayerDieState die;
    public PlayerAttackState attack;
    #endregion

    #region Private Field
    private IPlayerAnimState state;
    #endregion

    #region Mono
    private void Awake()
    {
        eventData = new PointerEventData(EventSystem.current);
        LookAt(Vector2.right);
        state = move;
    }
    private void Update()
    {
        State.InputProcess();
    }
    #endregion

    #region Method
    public void LookAt(Vector2 dir)
    {
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
    #endregion
}
