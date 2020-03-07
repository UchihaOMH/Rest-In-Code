using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerAttackState : OfficeWorkerState, IAnimState
{
    public Transform rangeBox;

    public float attackSpeed = 1f;
    public float coolTime = 0.5f;

    private float timer = 0f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rangeBox.position, rangeBox.lossyScale);
    }

    public string GetStateName()
    {
        return "Attack State";
    }
    public void Process()
    {
        var hit = Physics2D.BoxCast(rangeBox.position, new Vector2(Mathf.Abs(rangeBox.lossyScale.x), Mathf.Abs(rangeBox.lossyScale.y)), 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            if (Time.time >= timer)
            {
                officeWorker.apPortrait.SetAnimationSpeed(_OfficeWorkerAnimTrigger_.attack, attackSpeed);
                officeWorker.apPortrait.Play(_OfficeWorkerAnimTrigger_.attack);
                timer = Time.time + coolTime;
            }
        }
        else if (!officeWorker.apPortrait.IsPlaying(_OfficeWorkerAnimTrigger_.attack))
            officeWorker.CurrState = officeWorker.animationState.trace;
    }

    #region Animation Event
    public void OnAttack()
    {
        var hit = Physics2D.BoxCast(rangeBox.position, new Vector2(Mathf.Abs(rangeBox.lossyScale.x), Mathf.Abs(rangeBox.lossyScale.y)), 0f, Vector2.zero, 0f, LayerMask.GetMask(GameConst.LayerDefinition.player));
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Entity>().BeAttacked(officeWorker, officeWorker.info.damage, hit.collider.transform.position - officeWorker.tr.position, officeWorker.info.knockBackPower, 0.2f);
        }
    }
    public void OnAttackExit()
    {
        officeWorker.apPortrait.Play(_OfficeWorkerAnimTrigger_.idle);
    }
    #endregion
}
