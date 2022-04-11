using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    Random,
    Lich,
    Knight,
    Ninja,
}

public class GlobalValue : MonoBehaviour
{
    public static int Game_Stage = 0;
    public static int Game_Speed = 1;
    public static bool Game_End = false;

    public static int MyGold = 20;
    public static int MyLife = 10;
    public static int MyGem = 2;

    public static int Spawn_Mon_Cnt = 0;
    public static int Remain_Monster = 0;

    public static string Tower_Class = "없음";
    public static int Tower_Level = 0;
    public static int Tower_AtkDmg = 0;
    public static float Tower_AtkSpd = 0;
    public static float Tower_AtkRange = 0;
    public static string Tower_Skill_Name = "스킬 설명";
    public static string Tower_Skill_Image_Name = "";
    public static string Tower_Skill_Type = "";
    public static string Tower_Skill_Ex = "";

    public static int Knight_TW_LV = 1;
    public static int Lich_TW_LV = 1;
    public static int Ninja_TW_LV = 1;
}
