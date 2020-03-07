using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCinematic : MonoBehaviour
{
    private enum eendingPoint
    {
        FrontOfNPC = 0
    }

    public List<Transform> endingPoint = new List<Transform>();
    [Header("Component Reference"), Space(15f)]
    public Bible bible;
    public Girl girl;
    public Ending ending;

    [Header("Parameter"), Space(15f)]
    public bool endCinematicExit = false;

    private ControlPadInputModule tempInputModule;

    private bool onEnding = false;
    private Vector3 dest;
    private int endingPointIdx = 0;
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.MainUI.SetActive(false);
            player.inputModule = tempInputModule;
            onEnding = true;
        }
    }
    private void Start()
    {
        player = GameManager.Instance.Player;
        tempInputModule = GetComponentInChildren<ControlPadInputModule>();
        dest = endingPoint[(int)eendingPoint.FrontOfNPC].position;
    }
    private void Update()
    {
        if (!onEnding)
            return;

        if (dest.x - player.tr.position.x < player.info.speed * Time.deltaTime)
        {
            if (dest == endingPoint[(int)eendingPoint.FrontOfNPC].position)
            {
                GameManager.Instance.Conversation.StartConversation(gameObject, "Script/Main/Ending", () => OnCinematicExit());
            }

            dest = endingPoint[endingPointIdx < endingPoint.Count - 1 ? ++endingPointIdx : endingPointIdx].position;
            tempInputModule.CurrDir = new KeyValuePair<string, Touch>("", new Touch());
        }
        else
        {
            Touch touch = new Touch();
            touch.phase = TouchPhase.Stationary;
            tempInputModule.CurrDir = new KeyValuePair<string, Touch>("Right", touch);
        }
    }
    private void BibleAttackPlayer()
    {
        var hit = GameManager.Instance.FxPool.GetHitFXObject();
        hit.transform.position = player.GetComponent<BoxCollider2D>().ClosestPoint(bible.transform.position);
        hit.SetActive(true);
    }
    private void OnCinematicExit()
    {
        endCinematicExit = true;
        ending.ShowEnding();
    }
}
