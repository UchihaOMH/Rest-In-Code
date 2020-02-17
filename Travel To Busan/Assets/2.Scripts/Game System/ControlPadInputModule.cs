using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlPadInputModule : MonoBehaviour
{
    #region Property
    public KeyValuePair<string, Touch> CurrDir
    {
        get => currDir;
        set => currDir = value;
    }
    
    public KeyValuePair<bool, Touch> AttackButtonPressed
    {
        get => attackButtonPressed;
        set => attackButtonPressed = value;
    }
    public KeyValuePair<bool, Touch> SkillButtonPressed
    {
        get => skillButtonPressed;
        set => skillButtonPressed = value;
    }
    public KeyValuePair<bool, Touch> JumpButtonPressed
    {
        get => jumpButtonPressed;
        set => jumpButtonPressed = value;
    }
    #endregion

    #region Public Field
    
    #endregion

    #region Private Field
    private KeyValuePair<string, Touch> currDir;
    private KeyValuePair<bool, Touch> attackButtonPressed;
    private KeyValuePair<bool, Touch> jumpButtonPressed;
    private KeyValuePair<bool, Touch> skillButtonPressed;

    private PointerEventData eventData;
    private GraphicRaycaster raycaster;

    [SerializeField] private Transform controlStick;
    [SerializeField] private GameObject controlPad;
    [SerializeField] private GameObject attackButton;
    [SerializeField] private GameObject skillButton;
    [SerializeField] private GameObject jumpButton;

    private int controlPadTouchIndex = -1;
    #endregion

    private void Awake()
    {
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventData = new PointerEventData(EventSystem.current);
    }
    private void Update()
    {
        InterpretInput();
        HoldControlStick();
    }

    /// <summary>
    /// 인풋을 해석해서 프로퍼티를 초기화 하는 역할
    /// </summary>
    private void InterpretInput()
    {
        //  터치 갱신을 위한 플래그
        bool controlPadTouched = false;
        bool attackButtonTouched = false;
        bool skillButtonTouched = false;
        bool jumpButtonTouched = false;

        //  터치 레이캐스팅을 통한 프로퍼티 초기화
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            eventData.position = touch.position;
            List<RaycastResult> set = new List<RaycastResult>();
            raycaster.Raycast(eventData, set);

            if (set.Count > 0)
            {
                GameObject hitObject = set[0].gameObject;

                if (hitObject == controlPad || touch.fingerId == controlPadTouchIndex)
                {
                    controlPadTouched = true;

                    controlPadTouchIndex = touch.fingerId;

                    //  방향 해석
                    Vector2 dir = (controlStick.position - controlPad.transform.position).normalized;
                    float rot = Vector2.SignedAngle(Vector2.up, dir);
                    string interpretedDir = "";

                    if (Mathf.Abs(rot) < 20f)
                    {
                        interpretedDir = "Up";
                    }
                    else if (Mathf.Abs(rot) < 160f)
                    {
                        if (rot > 0f)
                            interpretedDir = "Left";
                        else if (rot < 0f)
                            interpretedDir = "Right";
                    }
                    else if (Mathf.Abs(rot) <= 180f)
                    {
                        interpretedDir = "Down";
                    }

                    CurrDir = new KeyValuePair<string, Touch>(interpretedDir, touch);
                }
                else if (hitObject == attackButton)
                {
                    attackButtonTouched = true;

                    AttackButtonPressed = new KeyValuePair<bool, Touch>(true, touch);
                }
                else if (hitObject == skillButton)
                {
                    skillButtonTouched = true;

                    SkillButtonPressed = new KeyValuePair<bool, Touch>(true, touch);
                }
                else if (hitObject == jumpButton)
                {
                    jumpButtonTouched = true;

                    JumpButtonPressed = new KeyValuePair<bool, Touch>(true, touch);
                }
            }
        }

        //  터치되지 않았을경우 프로퍼티 갱신
        if (!controlPadTouched)
        {
            CurrDir = new KeyValuePair<string, Touch>("", GameConst.emptyTouch);
            controlPadTouchIndex = -1;
        }
        if (!attackButtonTouched)
        {
            AttackButtonPressed = new KeyValuePair<bool, Touch>(false, GameConst.emptyTouch);
        }
        if (!skillButtonTouched)
        {
            SkillButtonPressed = new KeyValuePair<bool, Touch>(false, GameConst.emptyTouch);
        }
        if (!jumpButtonTouched)
        {
            JumpButtonPressed = new KeyValuePair<bool, Touch>(false, GameConst.emptyTouch);
        }
    }
    public void HoldControlStick()
    {
        if (Input.touchCount == 0 || controlPadTouchIndex == -1)
        {
            controlStick.position = controlPad.transform.position;
        }
        else if (controlPadTouchIndex != -1)
        {
            float radius = controlPad.GetComponent<RectTransform>().rect.size.y / 2f;
            Debug.DrawRay(controlPad.transform.position, Vector3.up * radius);

            float dist = Vector3.Distance(CurrDir.Value.position, controlPad.transform.position);
            if (dist <= radius)
            {
                controlStick.position = CurrDir.Value.position;
            }
            else
            {
                Vector3 dir = (CurrDir.Value.position - new Vector2(controlPad.transform.position.x, controlPad.transform.position.y)).normalized;
                controlStick.position = dir * radius;
            }
        }
    }
}
