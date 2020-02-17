using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerAttackState : OfficeWorkerState, IAnimState
{
    public float coolTime = 0.5f;
    public float timer = 0f;

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(attackRangeBox.position, attackRangeBox.lossyScale);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (officeWorker.CurrState == this)
        {
            if (collision.GetComponent<Player>() != null)
            {
                collision.GetComponent<Player>().BeAttacked(officeWorker, officeWorker.info.damage, collision.transform.position.x - officeWorker.tr.position.x < 0f ? Vector2.left : Vector2.right, officeWorker.knockBackDist);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (officeWorker.CurrState == this)
        {
            if (collision.GetComponent<Player>() != null)
            {
                collision.GetComponent<Player>().BeAttacked(officeWorker, officeWorker.info.damage, collision.transform.position.x - officeWorker.tr.position.x < 0f ? Vector2.left : Vector2.right, officeWorker.knockBackDist);
            }
        }
    }

    private void Update()
    {
        ////  공격판정 타이밍
        //if (attackable)
        //{
        //    var hit = Physics2D.BoxCast(attackRangeBox.position, attackRangeBox.lossyScale, 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        //    if (hit.collider != null && hit.collider.GetComponent<Player>() != null)
        //    {
        //        hit.collider.gameObject.GetComponent<Player>().BeAttacked(officeWorker, officeWorker.info.damage, hit.collider.transform.position.x - officeWorker.tr.position.x < 0f ? Vector2.left : Vector2.right, officeWorker.knockBackDist);
        //    }
        //}
    }

    public string GetStateName()
    {
        return "Attack State";
    }
    public void Process()
    {
        
    }

    public void Attack()
    {
        if (Time.time >= timer)
        {
            officeWorker.apPortrait.Play(_OfficeWorkerAnimTrigger_.attack);
            timer = Time.time + coolTime;
        }
    }

    #region Animation Event
    public void OnAttack(bool _attack)
    {
        GetComponent<BoxCollider2D>().enabled = _attack;
    }
    public void OnAttackExit()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        officeWorker.TransitionProcess(officeWorker.animationState.trace);
    }
    #endregion
}
