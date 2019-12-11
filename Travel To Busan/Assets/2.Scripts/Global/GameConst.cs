using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    public static class AnimationParameter
    {
        public const string bJump = "bJump";
        public const string bSit = "bSit";

        public const string fMoveBlend = "fMoveBlend";
        public const string fJumpBlend = "fJumpBlend";

        public const string tRoll = "tRoll";
        public const string tDie = "tDie";
        public const string tResurrection = "tResurrection";
        public const string tAttack = "tAttack";

        public const string iComboCount = "iComboCounter";
    }

    public static class LayerDefinition
    {
        public const string level = "Level";
        public const string controller = "Controller";
    }
}
