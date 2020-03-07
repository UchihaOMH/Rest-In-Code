using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSceneManager : SceneManagerClass
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

    #region Public Field
    public List<Transform> enemyPoint;

    [Header("Cinematic"), Space(15f)]
    public List<Transform> cinematicPoint;
    #endregion

    #region Private Field
    //  Component Reference
    private Player player;
    private ControlPadInputModule tempInputModule;

    //  Parameter
    private bool onCinematic = false;
    private Vector3 dest;
    private int cinematicPointIdx = 0;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.MainUI.SetActive(true);

            GameManager.Instance.Player.inputModule = GameManager.Instance.MainUI.GetComponentInChildren<ControlPadInputModule>();
            GameManager.Instance.Fade.LoadScene((int)GameManager.eScene.Level1, () =>
            {
                GameManager.Instance.EnemyPool.ReturnAllManagedEnemy();
                GameManager.Instance.MainUI.SetActive(true);

                GameManager.Instance.GameData.prologueShown = true;
                player.info.currHP = player.info.maxHP;

                fadeCallback();
            });
        }
    }
    private void Start()
    {
        GameManager.Instance.MainUI.SetActive(false);

        player = GameManager.Instance.Player;
        tempInputModule = GetComponentInChildren<ControlPadInputModule>();
        player.inputModule = tempInputModule;
        
        player.tr.position = startPoint.position;
        dest = cinematicPoint[++cinematicPointIdx].position;

        foreach (var point in enemyPoint)
            enemyList.Add(GameManager.Instance.EnemyPool.SpawnEnemy(point.position, "Office Worker").GetComponent<Enemy>());

        Camera.main.GetComponent<CameraControl>().ExplosionShake(1.5f, 1.5f);

        Invoke("StartCine", 1.5f);
    }
    private void Update()
    {
        if (!onCinematic)
            return;

        //  목적지에 도착하면
        if (cinematicPoint[cinematicPointIdx].position.x - player.tr.position.x < player.info.speed * Time.deltaTime)
        {
            //  대쉬 포인트. 유리를 부수기 위해 Speed를 두배가량 높인다.
            if (dest.x == cinematicPoint[(int)ePoint.DashPoint].position.x)
            {
                player.info.speed = 36f;
            }
            //  좀비 추격 시작 포인트. 좀비들이 추적하기 시작한다.
            else if (dest.x == cinematicPoint[(int)ePoint.JombieTracePoint].position.x)
            {
                StartCoroutine(ZombieTracingCourtine());
            }
            //  점프 포인트. 유리를 부수고 점프한다.
            else if (dest.x == cinematicPoint[(int)ePoint.JumpPoint].position.x)
            {
                player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
            }
            //  감속 포인트. 유리를 부수기 위해 가속한 만큼 감속한다.
            else if (dest.x == cinematicPoint[(int)ePoint.BrakePoint].position.x)
            {
                player.info.speed = 18f;
            }
            //  Rizing Attack 포인트. 앞을 막은 적을 뚫기 위해 공격한다
            else if (dest.x == cinematicPoint[(int)ePoint.RizingAttackPoint].position.x)
            {
                player.CurrState = player.animationStates.skill;
                (player.CurrState as PlayerSkillState).RizingAttack();
            }
            //  최종 포인트. 다음맵으로 이동한다.
            else if (dest.x == cinematicPoint[(int)ePoint.GoalPoint].position.x)
            {
                //  콜라이더가 포탈에 닿으면 씬 매니저가 씬전환시킴
            }

            dest = cinematicPoint[cinematicPointIdx < cinematicPoint.Count - 1 ? ++cinematicPointIdx : cinematicPointIdx].position;
        }

        Touch controlPadTouch = new Touch();
        controlPadTouch.phase = TouchPhase.Stationary;
        player.inputModule.CurrDir = new KeyValuePair<string, Touch>("Right", controlPadTouch);
    }

    private void StartCine()
    {
        onCinematic = true;
        //GameManager.Instance.Conversation.StartConversation("Script/Main/Prologue", () =>
        //{
        //    onCinematic = true;
        //});
    }

    private IEnumerator ZombieTracingCourtine()
    {
        int maxZombieCount = 10;

        for (int i = 0; i < maxZombieCount; i++)
        {
            var emy = GameManager.Instance.EnemyPool.SpawnEnemy(cinematicPoint[(int)ePoint.StartPoint].position, "Office Worker").GetComponent<OfficeWorker>();
            emy.FocusTarget(player);
            emy.info.speed = Random.Range(emy.info.speed * 0.9f, emy.info.speed * 1.1f);
            emy.TransitionProcess(emy.GetComponent<OfficeWorker>().animationState.trace);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}
