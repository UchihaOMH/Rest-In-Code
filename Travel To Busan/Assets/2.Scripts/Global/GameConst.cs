using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    public static readonly Touch emptyTouch = new Touch();

    public static class AnimationParameter
    {
        public const string bJump = "bJump";
        public const string bSit = "bSit";
        public const string bAlive = "bAlive";
        public const string bAttack = "bAttack";
        public const string bBasicAttack = "bBasicAttack";

        public const string fRunBlend = "fRunBlend";
        public const string fJumpBlend = "fJumpBlend";

        public const string tRoll = "tRoll";
        public const string tUpperCommand = "tUpperCommand";
        public const string tLowerCommand = "tLowerCommand";
        public const string tBreakCommand = "tBreakCommand";
    }

    public static class LayerDefinition
    {
        public const string level = "Level";
        public const string controller = "Controller";
    }
}
