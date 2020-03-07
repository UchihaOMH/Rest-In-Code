using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManagedObject
{
    Transform Pool
    {
        get;
        set;
    }

    void ReturnObject2Pool();
    void ResetObjectForPooling();
}
