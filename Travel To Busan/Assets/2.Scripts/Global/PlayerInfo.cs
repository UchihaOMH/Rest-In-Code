using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 데이터 정보 번들 (미완성 레벨업과, 초기화단계에서 MaxHP같은 수치 계수 설정)
/// </summary>
public class PlayerInfo
{
    #region Property
    public short Level { get => level; set => level = value; }
    public short Strength { get => strength; set => strength = value; }
    public short Chest { get => chest; set => chest = value; }
    public short Heart { get => heart; set => heart = value; }
    public short UnderBody { get => underBody; set => underBody = value; }

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Hp { get => hp; set => hp = value; }
    public float MaxAp { get => maxAp; set => maxAp = value; }
    public float Ap { get => ap; set => ap = value; }
    public float MaxExp { get => maxExp; set => maxExp = value; }
    public float Exp { get => exp; set => exp = value; }
    public float Speed { get => speed; set => speed = value; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }

    public string Name { get => name; set => name = value; }
    #endregion

    #region Private Field
    [TinyJSON.Include]
    private float maxHp = 0;
    [TinyJSON.Include]
    private float hp;
    [TinyJSON.Include]
    private float maxAp = 0;
    [TinyJSON.Include]
    private float ap;
    [TinyJSON.Include]
    private float maxExp = 0;
    [TinyJSON.Include]
    private float exp;
    [TinyJSON.Include]
    private float speed = 0.5f;
    [TinyJSON.Include]
    private float jumpForce = 30;

    [TinyJSON.Include]
    private short level;
    [TinyJSON.Include]
    private short strength;
    [TinyJSON.Include]
    private short chest;
    [TinyJSON.Include]
    private short heart;
    [TinyJSON.Include]
    private short underBody;

    [TinyJSON.Include]
    private string name = string.Empty;
    #endregion

    public PlayerInfo() { }
    public PlayerInfo(float maxHp, float maxAp, float maxExp,
        short level, short strength, short chest, short heart, short underBody, string name)
    {
        this.MaxHp = maxHp;
        this.MaxAp = maxAp;
        this.MaxExp = maxExp;

        Hp = MaxHp;
        Ap = maxAp;
        Exp = 0;

        this.Level = level;
        this.Strength = strength;
        this.Chest = chest;
        this.Heart = heart;
        this.UnderBody = underBody;

        this.Name = name;
    }

    #region Method
    public void CopyPlayerInfo(PlayerInfo copyTo)
    {
        this.maxHp = copyTo.MaxHp;
        this.Hp = copyTo.Hp;
        this.maxAp = copyTo.MaxAp;
        this.Ap = copyTo.Ap;
        this.MaxExp = copyTo.MaxExp;
        this.Exp = copyTo.Exp;
        this.Speed = copyTo.speed;
        this.JumpForce = copyTo.jumpForce;

        this.Level = copyTo.level;
        this.Strength = copyTo.strength;
        this.Chest = copyTo.chest;
        this.Heart = copyTo.heart;
        this.UnderBody = copyTo.underBody;

        this.name = copyTo.name;
    }

    public void LevelUP()
    {
        Level++;

        strength += GameConst.IncreaseStatPerLevel.strength;
        chest += GameConst.IncreaseStatPerLevel.chest;
        heart += GameConst.IncreaseStatPerLevel.heart;
        underBody += GameConst.IncreaseStatPerLevel.underBody;
    }

    public override string ToString()
    {
        string str = string.Format("Max HP : {0}\n" +
            "HP : {1}\n" +
            "Max AP : {2}\n" +
            "AP : {3}\n" +
            "Max EXP : {4}\n" +
            "EXP : {5}\n" +
            "speed : {6}\n" +
            "jumpForce : {7}\n" +
            "level : {8}\n" +
            "strength : {9}\n" +
            "chest : {10}\n" +
            "heart : {11}\n" +
            "underBody : {12}\n" +
            "name : {13}", maxHp, Hp, MaxAp, Ap, MaxExp, Exp, Speed, JumpForce, Level, Strength, Chest, Heart, UnderBody, Name);

        return str;
    }
    #endregion
}
