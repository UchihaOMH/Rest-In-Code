using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전역 상수 제공
/// </summary>
public static class GameConst
{
    public enum eSpriteDirection
    {
        Left,
        Right
    }

    public static PlayerInfo newPlayerInfo = new PlayerInfo(300, 100, 500, 1, 30, 30, 30, 30, string.Empty);

    public static class AnimationParam
    {
        public enum eState
        {
            Run,
            Idle,
            Die,
            Jump,
            Fall,
            Sit,
            Roll,
            Attack
        }

        public const string run = "tRun";
        public const string idle = "tIdle";
        public const string die = "tDie";
        public const string jump = "tJump";
        public const string fall = "tFall";
        public const string sit = "tSit";
        public const string roll = "tRoll";
        public const string attack = "tAttack";

        public const string comboCount = "iComboCount";
    }
    public static class AnimationName
    {
        public const string run = "Run";
        public const string idle = "Idle";
        public const string die = "Die";
        public const string jump = "Jump";
        public const string fall = "Fall";
        public const string sit = "Sit";
        public const string roll = "Roll";

        public const string attack = "Attack";
    }
    public static class IncreaseStatPerLevel
    {
        public static short strength = 30;
        public static short chest = 10;
        public static short heart = 50;
        public static short underBody = 5;
    }
    public static class FirebasePath
    {
        /// <summary>
        /// key : user.id   value : PlayerInfo Type
        /// </summary>
        public static string playerData = "Player/ID/";
    }
}
