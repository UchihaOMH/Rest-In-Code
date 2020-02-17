using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 알고리즘 개선 필요
/// </summary>
public class OfficeWorkerPatrolState : OfficeWorkerState, IAnimState
{
    public Transform detectRange;
    public Vector2 raycastOffset;

    [SerializeField] private float xPoint;
    private float moveCoolTime = 0f;

    private void Awake()
    {
        xPoint = officeWorker.wanderOrigin.x;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectRange.position, detectRange.lossyScale.x / 2f);
    }

    public string GetStateName()
    {
        return "Patrol State";
    }
    public void Process()
    {
        Patrol();
    }

    public void Patrol()
    {
        //  주변 적 탐지.
        if (DetectTarget())
            return;

        //  탐지 후에도 타깃이 null일 경우 정찰
        if (officeWorker.target == null)
        {
            //  첫 목적지 설정
            if (xPoint == 0f)
                xPoint = Random.Range(officeWorker.wanderOrigin.x - officeWorker.wanderRadius, officeWorker.wanderOrigin.x + officeWorker.wanderRadius);

            //  이동 쿨타임이 돌았으면 정찰 개시
            if (Time.time >= moveCoolTime)
            {
                //  목적지 도착시, 쿨타임 갱신 및 애니메이션 Idle로 설정
                if (Mathf.Abs(xPoint - officeWorker.tr.position.x) <= officeWorker.patrolSpeed)
                {
                    moveCoolTime = Time.time + Random.Range(0f, 3f);
                    //  여기서 절벽 판정을 해야함
                    xPoint = Random.Range(officeWorker.wanderOrigin.x - officeWorker.wanderRadius, officeWorker.wanderOrigin.x + officeWorker.wanderRadius);
                    Vector3 dir = new Vector3((xPoint - officeWorker.tr.position.x < 0f ? -1f : 1f), 0f, 0f);
                    officeWorker.LookAt(dir);

                    if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.idle))
                        officeWorker.apPortrait.CrossFade(_OfficeWorkerAnimTrigger_.idle);
                }
                else
                {
                    //  이동 로직 외 앞에 절벽일 경우 목적지를 변경 (개선 필요 : 절벽 판정을 이동중에 하면 안됌)
                    var collider = officeWorker.GetComponent<BoxCollider2D>();
                    var colliderEndPoint = new Vector2(collider.bounds.max.x, collider.bounds.min.y);

                    var hit = Physics2D.BoxCast(new Vector2(colliderEndPoint.x + collider.bounds.size.x / 2f, colliderEndPoint.y), collider.size, 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.level));
                    if (hit.collider != null)
                    {
                        Vector3 dir = new Vector3((xPoint - officeWorker.tr.position.x < 0f ? -1f : 1f), 0f, 0f);
                        officeWorker.LookAt(dir);
                        officeWorker.tr.Translate(dir.normalized * officeWorker.patrolSpeed, Space.World);
                        
                        if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.patrol))
                            officeWorker.apPortrait.CrossFade(_OfficeWorkerAnimTrigger_.patrol);
                    }
                    else
                    {
                        if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.idle))
                            officeWorker.apPortrait.CrossFade(_OfficeWorkerAnimTrigger_.idle);
                        xPoint = officeWorker.tr.position.x;
                        return;
                    }
                }
            }
        }
    }
    public bool DetectTarget()
    {
        var hit = Physics2D.CircleCast(detectRange.position, detectRange.lossyScale.x / 2f, Vector3.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            //  타깃이 없을 때, 플레이어가 감지되면 타깃설정 및 추적상태로 변경
            if (officeWorker.target == null)
            {
                officeWorker.target = hit.collider.GetComponent<Player>();
                officeWorker.TransitionProcess(officeWorker.animationState.trace);
            }
            return true;
        }
        return false;
    }
}
