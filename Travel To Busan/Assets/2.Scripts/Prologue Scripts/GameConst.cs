using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    public enum SpriteDefaultDirection
    {
        Left,
        Right
    }

    public static class AnimationParam
    {
        public enum eAnim
        {
            Run,
            Idle,
            Die,
            Jump,
            Sit,
            Roll,
            Punch,
            Kick
        }

        #region Animator Parameters
        public const string tRun = "tRun";
        public const string tIdle = "tIdle";
        public const string tDie = "tDie";
        public const string tJump = "tJump";
        public const string tSit = "tSit";
        public const string tRoll = "tRoll";
        public const string tKick = "tKick";
        public const string tPunch = "tPunch";
        public const string tResurrect = "tResurrect";

        public const string iComboCount = "iComboCount";
        #endregion
    }
}
