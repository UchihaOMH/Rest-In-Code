using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyPortrait;

public class Girl : MonoBehaviour
{
    private apPortrait apPortrait;

    private void Start()
    {
        apPortrait = GetComponentInChildren<apPortrait>();
        apPortrait.Initialize();
        apPortrait.Play("Idle");
    }
}
