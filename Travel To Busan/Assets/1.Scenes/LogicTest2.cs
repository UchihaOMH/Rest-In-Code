using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTest2 : LogicTest
{
    private void Start()
    {
        Debug.Log("Child Class Starting");
    }
    private void Update()
    {
        Debug.Log("Child Class Updating");
    }
}
