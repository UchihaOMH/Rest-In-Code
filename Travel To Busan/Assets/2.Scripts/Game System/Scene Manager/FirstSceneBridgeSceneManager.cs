using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneBridgeSceneManager : SceneManagerClass
{
    private void Start()
    {
        GameManager.Instance.MainUI.SetActive(false);
    }
}
