using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    [SerializeField] public bool prologueShown = false;
    //  set true on Level1SceneManager class
    [SerializeField] public bool uiTutorialShown = false;
    [SerializeField] public int currLevel = 0;
    [SerializeField] public _EntityInfo_ playerInfo;

    public override string ToString()
    {
        string str = "DEBUG : \n" +
            "Prologue Shown : " + prologueShown +
            "UI Tutorial Shown : " + uiTutorialShown +
            "Current Level : " + currLevel;

        return str;
    }
}
