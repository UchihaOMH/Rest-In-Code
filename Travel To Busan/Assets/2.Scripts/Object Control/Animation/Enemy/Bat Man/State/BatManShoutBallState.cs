using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatManShoutBallState : BatManState
{
    public GameObject GetBall
    {
        get
        {
            foreach (var ball in ballList)
            {
                if (!ball.activeSelf)
                    return ball;
            }
            GameObject newBall = Instantiate(ballList[0], transform);
            newBall.GetComponent<ThrowableObject>().Pool = transform;
            ballList.Add(newBall);
            return newBall;
        }
    }
    [SerializeField] private List<GameObject> ballList = new List<GameObject>();

    public float coolTime = 2f;
    public Transform handSocket;

    private GameObject currBall;

    private void OnEnable()
    {
        foreach (var ball in ballList)
            ball.GetComponent<ThrowableObject>().Pool = transform;
    }

    public override string GetStateName()
    {
        return "Bat Man Shout Ball State";
    }
    public override void Process()
    {
        if (!batMan.apPortrait.IsPlaying(BatMan._BatManAnim_.shoutBall))
        {
            batMan.apPortrait.Play(BatMan._BatManAnim_.shoutBall);
            batMan.LookAt((batMan.Target.transform.position - batMan.transform.position).x < 0f ? Vector2.left : Vector2.right);
        }
    }

    #region Animation Event
    public void OnAttackEnter()
    {
        currBall = GetBall;
        currBall.SetActive(true);
        currBall.transform.SetParent(handSocket);
        currBall.transform.localPosition = Vector3.zero;
        currBall.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    public void OnAttack()
    {
        currBall.transform.SetParent(transform);
        currBall.GetComponent<ThrowableObject>().ThrowTo((batMan.Target.transform.position - handSocket.position).normalized);
    }
    public void OnAttackExit()
    {
        batMan.animState.patternBridgeState.timer = Time.time + coolTime;
        batMan.TransitionProcess(batMan.animState.patternBridgeState);
    }
    #endregion
}
