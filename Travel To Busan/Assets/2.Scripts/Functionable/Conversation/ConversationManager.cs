using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 지정된 포멧으로 작성된 txt파일을 받아서, 대화형식으로 출력함
/// </summary>
public class ConversationManager : MonoBehaviour
{
    [System.Serializable]
    public struct _Illust_
    {
        public Sprite player;
        public Sprite zeroU;
        public Sprite bible;
        public Sprite girl;

        [Space(15f)]
        public Sprite officeWorker;
        public Sprite underArmourCollector;
        public Sprite batMan;
    }
    #region Property
    public Talker CurrTalker
    {
        get => currTalker;
        set => currTalker = value;
    }
    #endregion

    #region Public Field
    [Header("Reference")]
    public Text textBox;
    public Image leftImage;
    public Image rightImage;

    [Header("Parameter"), Space(20f)]
    public _Illust_ spriteIllust;
    public bool onConversation = false;
    #endregion

    #region Private Field
    private Talker currTalker;

    private Talker leftTalker;
    private Talker rightTalker;

    private Action callback;

    //  Raycast
    private GraphicRaycaster raycaster;
    private PointerEventData eventData;

    private List<string> conversation = new List<string>();
    private int conversationIndexer = 0;
    private GameObject eventObject;

    private static string flagPattern = "^\\[Direction=\".*?\",Illust Name=\".*?\"(,Method=\".*?\")?\\]";
    private static string illustNamePattern = "(?<=Illust Name=\").*?(?=\")";
    private static string directionPattern = "(?<=Direction=\").*?(?=\")";
    private static string methodPattern = "(?<=Method=\").*?(?=\")";
    #endregion

    private void Awake()
    {
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventData = new PointerEventData(EventSystem.current);

        leftImage.transform.localScale = new Vector3(-1f, 1f, 1f);

        leftTalker = new Talker(leftImage, textBox);
        rightTalker = new Talker(rightImage, textBox);

        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (onConversation)
        {
#if UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                DisplayNextConversation();
            }
#endif
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                DisplayNextConversation();
            }
#endif
        }
    }
    private void OnDisable()
    {
        conversation = null;
        conversationIndexer = 0;
        callback = null;
        onConversation = false;
        currTalker = null;
    }

    //  Public :
    /// <summary>
    /// If converstation has other work already, return false
    /// </summary>
    /// <param name="eventObject">Gameobject what having the Method what script flag specified</param>
    /// <param name="_dbUri">Generally "Script/Main/[Name]"</param>
    /// <param name="_callback">On Conversation Exit Event</param>
    /// <returns></returns>
    public bool StartConversation(GameObject _eventObject, string _dbUri, Action _callback = null)
    {
        if (onConversation)
            return false;

        eventObject = _eventObject;
        callback = _callback;

        GameManager.Instance.DbConnection.LoadDataFromDB(_dbUri, (List<string> _conversation) =>
        {
            if (_conversation != default(List<string>))
            {
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0f;
                GameManager.Instance.MainUI.SetActive(false);
                gameObject.SetActive(true);
                onConversation = true;
                conversation = _conversation;
                DisplayNextConversation();
            }
            else
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                _callback?.Invoke();
            }
        });

        return true;
    }

    //  Private :
    private void DisplayNextConversation()
    {
        if (conversation.Count == conversationIndexer)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            GameManager.Instance.MainUI.SetActive(true);
            gameObject.SetActive(false);
            leftTalker.Leave();
            rightTalker.Leave();

            callback?.Invoke();
            return;
        }

        string text = conversation[conversationIndexer++];

        //  문자열이 플래그일 경우
        if (Regex.IsMatch(text, flagPattern))
        {
            string direction = Regex.Match(text, directionPattern).Value;
            string illustName = Regex.Match(text, illustNamePattern).Value;
            string method = Regex.Match(text, methodPattern).Value;

            //  왼쪽에서 말하는 경우
            if (direction == "Left")
            {
                //  Allignment설정, 일러스트 설정
                currTalker?.Wait();
                textBox.alignment = TextAnchor.MiddleLeft;
                currTalker = leftTalker;
                currTalker.SetIllust(GetIlustByName(illustName));
                leftImage.rectTransform.anchoredPosition = new Vector2(leftImage.rectTransform.rect.size.x, 0f);
            }
            //  오른쪽에서 말하는 경우
            else if (direction == "Right")
            {
                //  Allignment설정, 일러스트 설정
                currTalker?.Wait();
                textBox.alignment = TextAnchor.MiddleRight;
                currTalker = rightTalker;
                currTalker.SetIllust(GetIlustByName(illustName));
            }

            if (method != string.Empty)
                eventObject?.SendMessage(method);

            DisplayNextConversation();
            return;
        }
        else
        {
            if (currTalker == null)
                throw new NullReferenceException();

            CurrTalker.Speak(text);
        }
    }
    private Sprite GetIlustByName(string _name)
    {
        Sprite illust = null;

        switch (_name)
        {
            //  Servivor
            case "Player":
                illust = spriteIllust.player;
                break;
            case "ZeroU":
                illust = spriteIllust.zeroU;
                break;
            case "Bible":
                illust = spriteIllust.bible;
                break;
            case "Girl":
                illust = spriteIllust.girl;
                break;

            //  Monster
            case "Office Worker":
                illust = spriteIllust.officeWorker;
                break;
            case "UnderArmour Collector":
                illust = spriteIllust.underArmourCollector;
                break;
            case "Bat Man":
                illust = spriteIllust.batMan;
                break;
        }
        if (illust == null)
            throw new NullReferenceException("This llust name ware not defined : " + _name);

        return illust;
    }
}