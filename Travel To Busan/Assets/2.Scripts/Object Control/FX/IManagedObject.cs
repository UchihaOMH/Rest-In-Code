using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManagedObject
{
    Transform Pool
    {
        get;
    }

    void ReturnObject2Pool();
}
