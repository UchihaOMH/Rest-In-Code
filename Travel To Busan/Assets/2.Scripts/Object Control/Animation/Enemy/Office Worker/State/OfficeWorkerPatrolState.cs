using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerPatrolState : OfficeWorkerState, IAnimState
{
    public Vector2 raycastOffset;
    public float detectRange = 2f;

    [SerializeField] private float xPoint;
    private float moveCoolTime = 0f;

    private void Awake()
    {
        xPoint = officeWorker.wanderOrigin.x;
    }

    public string GetStateName()
    {
        return "Patrol State";
    }
    public void Process()
    {
        Vector2 dir = officeWorker.tr.rotation.y == 0f ? Vector2.left : Vector2.right;
        var hit = Physics2D.Raycast(officeWorker.tr.position, dir, detectRange, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            officeWorker.TransitionProcess(officeWorker.animationState.howl);
            (officeWorker.CurrState as OfficeWorkerHowlState).Howl(hit.collider?.GetComponent<Entity>());
            xPoint = 0f;
            return;
        }

        Patrol();
    }

    public void Patrol()
    {
        if (xPoint == 0f)
            xPoint = Random.Range(officeWorker.wanderOrigin.x - officeWorker.wanderRadius, officeWorker.wanderOrigin.x + officeWorker.wanderRadius);

        if (Time.time >= moveCoolTime)
        {
            if (Mathf.Abs(officeWorker.tr.position.x - xPoint) <= officeWorker.patrolSpeed)
            {
                moveCoolTime = Time.time + Random.Range(0f, 3f);
                xPoint = Random.Range(officeWorker.wanderOrigin.x - officeWorker.wanderRadius, officeWorker.wanderOrigin.x + officeWorker.wanderRadius);
                officeWorker.anim.SetFloat(_OfficeWorkerAnimTrigger_.fPatrolBlend, 0f);
            }
            else
            {
                Vector3 dir = new Vector3((xPoint - officeWorker.tr.position.x < 0f ? -1f : 1f), 0f, 0f);
                officeWorker.LookAt(dir);
                officeWorker.tr.Translate(dir.normalized * officeWorker.patrolSpeed, Space.World);
                officeWorker.anim.SetFloat(_OfficeWorkerAnimTrigger_.fPatrolBlend, 1f);
            }
        }
        //officeWorker.anim.SetFloat(_OfficeWorkerAnimTrigger_.fPatrolBlend, 1f);

        //if (patrolPoint == Vector3.zero)
        //{
        //    float min = officeWorker.wanderOrigin.x - officeWorker.wanderRadius;
        //    float max = officeWorker.wanderOrigin.x + officeWorker.wanderRadius;
        //    float xPos = Random.Range(min, max);
        //    patrolPoint = new Vector3(xPos, 0f, 0f);
            
        //}
        //else if (Mathf.Abs(officeWorker.tr.position.x - patrolPoint.x) < officeWorker.info.patrolSpeed)
        //{
        //    timer = Time.time + Random.Range(0f, 3f);
        //    patrolPoint = Vector3.zero;
        //    officeWorker.anim.SetFloat(_OfficeWorkerAnimTrigger_.fPatrolBlend, 0f);
        //}
        //else if (Time.time >= timer)
        //{
        //    Vector3 direction = new Vector3(patrolPoint.x - officeWorker.tr.position.x < 0f ? -1f : 1f, 0f, 0f);
        //    officeWorker.LookAt(direction);
        //    officeWorker.tr.Translate(direction * officeWorker.info.patrolSpeed, Space.World);
        //}
    }
}
