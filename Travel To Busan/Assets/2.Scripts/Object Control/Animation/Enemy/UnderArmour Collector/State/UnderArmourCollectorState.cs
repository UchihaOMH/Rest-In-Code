using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnderArmourCollectorState : MonoBehaviour, IAnimState
{
    public UnderArmourCollector collector;

    public abstract string GetStateName();
    public abstract void Process();
}
