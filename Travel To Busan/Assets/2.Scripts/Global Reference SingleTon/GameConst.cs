using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    public static readonly Touch emptyTouch = new Touch();
    public static readonly KeyValuePair<string, TouchPhase> emptyKeyValuePair = new KeyValuePair<string, TouchPhase>();

    public static class LayerDefinition
    {
        public const string level = "Level";
        public const string player = "Player";
        public const string enemy = "Enemy";
        public const string controller = "Controller";
    }
}

