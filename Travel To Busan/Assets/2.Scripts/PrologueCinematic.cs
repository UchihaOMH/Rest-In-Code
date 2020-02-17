using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueCinematic : MonoBehaviour
{
    public Transform startPos;
    public Transform enemyTracePos;
    public Transform jumpPos;
    public Transform endPos;

    private Vector3 dest;
    private Player player;
    private Transform playerTr;
    private ControlPadInputModule inputModule;

    private bool playerJumping = false;

    private bool cinematicStart = false;

    private void Start()
    {
        player = GameManager.Instance.player;
        playerTr = player.transform;
        playerTr.position = startPos.position;
        playerTr.localScale = new Vector3(-1f, 1f, 1f);

        inputModule = GetComponent<ControlPadInputModule>();

        dest = enemyTracePos.position;
    }
    private void Update()
    {
        if (!cinematicStart)
            return;

        //  목적지에 도착하면
        if (dest.x - playerTr.position.x < player.info.speed)
        {
            //  EnemyTracePos면 Enemy가 랜덤한 간격으로 생성되고, 따라오기 시작함
            if (dest.x == enemyTracePos.position.x)
            {
                dest = jumpPos.position;
                StartCoroutine(ZombieTracingCourtine());
            }
            //  점프포인트이면 점프하고 다음목적지 재설정
            else if (dest.x == jumpPos.position.x)
            {
                dest = endPos.position;
                player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
                playerJumping = true;
            }
        }

        playerTr.Translate(Vector3.right * player.info.speed, Space.World);

        //  점프 상태면
        if (playerJumping)
        {
            //  상승
            if (player.rb.velocity.y > 0.001f)
            {
                if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.jump))
                    player.apPortrait.Play(_PlayerAnimTrigger_.jump);
            }// 하락
            else if (player.rb.velocity.y < -0.001f)
            {
                if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.fall))
                    player.apPortrait.Play(_PlayerAnimTrigger_.fall);
            }// 착지
            else
            {
                playerJumping = false;
                if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.run))
                    player.apPortrait.Play(_PlayerAnimTrigger_.run);
            }
        }
        else
        {
            try
            {
                if (!player.apPortrait.IsPlaying(_PlayerAnimTrigger_.run))
                    player.apPortrait.Play(_PlayerAnimTrigger_.run);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }
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
            GameObject emy = GameManager.Instance.enemyPoolManager.SpawnEnemy(startPos.position, "Office Worker");
            emy.GetComponent<OfficeWorker>().target = player;
            emy.GetComponent<OfficeWorker>().info.speed = Random.Range(0.4f, 0.75f);    
            emy.GetComponent<OfficeWorker>().TransitionProcess(emy.GetComponent<OfficeWorker>().animationState.trace);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}
