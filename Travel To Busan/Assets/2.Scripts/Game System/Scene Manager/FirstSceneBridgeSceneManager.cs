using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneBridgeSceneManager : SceneManagerClass   
{
    private void Start()
    {
        if (GameManager.Instance.GameData.firstStart == true)
        {
            GameManager.Instance.GameData.firstStart = false;
            GameManager.Instance.GameData.currLevel = (int)GameManager.eScene.Prologue;
            GameManager.Instance.Fade.LoadScene((int)GameManager.eScene.Prologue);
        }
        else
        {
            GameManager.Instance.Fade.LoadScene(GameManager.Instance.GameData.currLevel);
        }
    }
}
