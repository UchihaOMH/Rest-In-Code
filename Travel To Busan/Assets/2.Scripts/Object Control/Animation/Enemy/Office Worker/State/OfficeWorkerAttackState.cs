using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerAttackState : OfficeWorkerState, IAnimState
{
    public string GetStateName()
    {
        return "Attack State";
    }
    public void Process()
    {
        throw new System.NotImplementedException();
    }
}
