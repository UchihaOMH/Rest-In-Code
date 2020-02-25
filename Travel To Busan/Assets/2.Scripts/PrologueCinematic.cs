using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueCinematic : MonoBehaviour
{
    private enum ePoint
    {
        StartPoint = 0,
        DashPoint,
        JombieTracePoint,
        JumpPoint,
        BrakePoint,
        RizingAttackPoint,
        GoalPoint,
    }

    public List<Transform> point = new List<Transform>();

    private Vector3 dest;
    private Player player;
    private Transform playerTr;
    private ControlPadInputModule inputModule;

    private bool cinematicStart = false;
    private int pointIndex = 0;

    private void Start()
    {
        player = GameManager.Instance.Player;
        playerTr = player.transform;
        playerTr.position = point[pointIndex].position;
        playerTr.localScale = new Vector3(-1f, 1f, 1f);

        inputModule = GetComponent<ControlPadInputModule>();

        dest = point[++pointIndex].position;
    }
    private void Update()
    {
        if (!cinematicStart)
            return;

        player.info.currHP = player.info.maxHP;

        //  목적지에 도착하면
        if (point[pointIndex].position.x - playerTr.position.x < player.info.speed * Time.deltaTime)
        {
            //  대쉬 포인트. 유리를 부수기 위해 Speed를 두배가량 높인다.
            if (dest.x == point[(int)ePoint.DashPoint].position.x)
            {
                player.info.speed = 36f;
            }
            //  좀비 추격 시작 포인트. 좀비들이 추적하기 시작한다.
            else if (dest.x == point[(int)ePoint.JombieTracePoint].position.x)
            {
                StartCoroutine(ZombieTracingCourtine());
            }
            //  점프 포인트. 유리를 부수고 점프한다.
            else if (dest.x == point[(int)ePoint.JumpPoint].position.x)
            {
                player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
            }
            //  감속 포인트. 유리를 부수기 위해 가속한 만큼 감속한다.
            else if (dest.x == point[(int)ePoint.BrakePoint].position.x)
            {
                player.info.speed = 18f;
            }
            //  Rizing Attack 포인트. 앞을 막은 적을 뚫기 위해 공격한다
            else if (dest.x == point[(int)ePoint.RizingAttackPoint].position.x)
            {
                player.TransitionProcess(player.animationStates.skill);
                (player.CurrState as PlayerSkillState).RizingAttack();
            }
            //  최종 포인트. 다음맵으로 이동한다.
            else if (dest.x == point[(int)ePoint.GoalPoint].position.x)
            {
                //  콜라이더가 포탈에 닿으면 씬 매니저가 씬전환시킴
            }

            dest = point[pointIndex < point.Count - 1 ? ++pointIndex : pointIndex].position;
        }

        Touch controlPadTouch = new Touch();
        controlPadTouch.phase = TouchPhase.Stationary;
        inputModule.CurrDir = new KeyValuePair<string, Touch>("Right", controlPadTouch);
    }

    public void StartCinematic()
    {
        cinematicStart = true;
    }

    private IEnumerator ZombieTracingCourtine()
    {
        int maxZombieCount = 10;

        for (int i = 0; i < maxZombieCount; i++)
        {
            var emy = GameManager.Instance.EnemyPool.SpawnEnemy(point[(int)ePoint.StartPoint].position, "Office Worker").GetComponent<OfficeWorker>();
            emy.target = player;
            emy.info.speed = Random.Range(emy.info.speed * 0.9f, emy.info.speed * 1.1f);
            emy.TransitionProcess(emy.GetComponent<OfficeWorker>().animationState.trace);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}
