using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeMgr : MonoBehaviour
{
    [SerializeField] Button Lich_Up_Btn;
    [SerializeField] Text Lich_Lv_Txt;
    [SerializeField] Text Lich_Up_Txt;
    [SerializeField] Button Knight_Up_Btn;
    [SerializeField] Text Knight_Lv_Txt;
    [SerializeField] Text Knight_Up_Txt;
    [SerializeField] Button Ninja_Up_Btn;
    [SerializeField] Text Ninja_Lv_Txt;
    [SerializeField] Text Ninja_Up_Txt;
    GameInfoCanvasMgr GICM;
    // Start is called before the first frame update
    void Start()
    {
        GICM = FindObjectOfType<GameInfoCanvasMgr>();
        Lich_Up_Txt.text = (GlobalValue.Lich_TW_LV * 40).ToString() + "Gold";
        Knight_Up_Txt.text = (GlobalValue.Knight_TW_LV * 40).ToString() + "Gold";
        Ninja_Up_Txt.text = (GlobalValue.Ninja_TW_LV * 200).ToString() + "Gold";

        Lich_Up_Btn.onClick.AddListener(() =>
        {
            if (GlobalValue.MyGold >= GlobalValue.Lich_TW_LV * 40)
            {
                GlobalValue.MyGold -= GlobalValue.Lich_TW_LV * 40;
                TowerUpgrade("Lich");
                Lich_Up_Txt.text = (GlobalValue.Lich_TW_LV * 40).ToString() + "Gold";
                Lich_Lv_Txt.text = "Lv." + GlobalValue.Lich_TW_LV.ToString();
            }
            else
            {
                GICM.InitSystemMsg("<color=#C6F300>Gold</color>");
            }
        });

        Knight_Up_Btn.onClick.AddListener(() =>
        {
            if (GlobalValue.MyGold >= GlobalValue.Knight_TW_LV * 40)
            {
                GlobalValue.MyGold -= GlobalValue.Knight_TW_LV * 40;
                TowerUpgrade("Knight");
                Knight_Up_Txt.text = (GlobalValue.Knight_TW_LV * 40).ToString() + "Gold";
                Knight_Lv_Txt.text = "Lv." + GlobalValue.Knight_TW_LV.ToString();
            }
            else
            {
                GICM.InitSystemMsg("<color=#C6F300>Gold</color>");
            }
        });

        Ninja_Up_Btn.onClick.AddListener(() =>
        {
            if (GlobalValue.MyGold >= GlobalValue.Ninja_TW_LV * 100)
            {
                GlobalValue.MyGold -= GlobalValue.Ninja_TW_LV * 100;
                TowerUpgrade("Junko");
                Ninja_Up_Txt.text = (GlobalValue.Ninja_TW_LV * 100).ToString() + "Gold";
                Ninja_Lv_Txt.text = "Lv." + GlobalValue.Ninja_TW_LV.ToString();
            }
            else
            {
                GICM.InitSystemMsg("<color=#C6F300>Gold</color>");
            }
        });
    }


    void TowerUpgrade(string name)
    {
        if (name == "Knight")
            GlobalValue.Knight_TW_LV++;
        else if (name == "Lich")
            GlobalValue.Lich_TW_LV++;
        else if (name == "Junko")
            GlobalValue.Ninja_TW_LV++;
    }
}