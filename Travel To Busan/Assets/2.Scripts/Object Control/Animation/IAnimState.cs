using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimState
{
    string GetStateName();
    void Process();
}
