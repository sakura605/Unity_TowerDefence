using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoMgr : MonoBehaviour
{
    [SerializeField] Text Tower_Class_Txt;
    [SerializeField] Text Tower_Level_Txt;
    [SerializeField] Text Tower_AtkDmg_Txt;
    [SerializeField] Text Tower_AtkSpd_Txt;
    [SerializeField] Text Tower_AtkRange_Txt;

    [SerializeField] Image Tower_Skill_Icon;
    [SerializeField] Text Tower_Skill_Name_Txt;
    [SerializeField] Text Tower_Skill_Type_Txt;
    [SerializeField] Text Tower_Skill_Ex_Txt;

    //[SerializeField] Button Learn_Skill_Btn;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Tower_Class_Txt.text = "클래스 : " + GlobalValue.Tower_Class;
        Tower_Level_Txt.text = "레벨 : " + GlobalValue.Tower_Level;
        Tower_AtkDmg_Txt.text = "공격력 : " + GlobalValue.Tower_AtkDmg;
        Tower_AtkSpd_Txt.text = "공격 속도 : " + GlobalValue.Tower_AtkSpd;
        Tower_AtkRange_Txt.text = "사정 거리 : " + GlobalValue.Tower_AtkRange;
        Tower_Skill_Icon.sprite = Resources.Load<Sprite>(GlobalValue.Tower_Skill_Image_Name);
        Tower_Skill_Name_Txt.text = GlobalValue.Tower_Skill_Name;
        Tower_Skill_Type_Txt.text = GlobalValue.Tower_Skill_Type;
        Tower_Skill_Ex_Txt.text = GlobalValue.Tower_Skill_Ex;
    }
}