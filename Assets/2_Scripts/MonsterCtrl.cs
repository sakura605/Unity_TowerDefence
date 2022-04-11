using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCtrl : MonoBehaviour
{
    GameObject[] MovePos;
    float MoveSpeed = 3.0f;
    int destination = 1;

    //몬스터 스탯 관련
    [SerializeField] Image Hp_Bar;
    [HideInInspector] public static float Max_HP;
    [HideInInspector] public float Mon_HP;

    GameObject Die_Ptc;
    // Start is called before the first frame update
    void Start()
    {
        Max_HP = GlobalValue.Game_Stage * 20;
        Mon_HP = Max_HP;
        MovePos = GameObject.FindGameObjectsWithTag("MovePos");
        Die_Ptc = Resources.Load<GameObject>("Particlecollection_Free samples/Prefab/Hit/Hit_06");
    }

    // Update is called once per frame
    void Update()
    {
        if (SkillMgr.IsRain) MoveSpeed = 1.5f;
        else MoveSpeed = 3.0f;

        SettingHpBar();
        Moving();
        Dying();
    }

    void Moving()
    {
        if (this.transform.position == MovePos[destination].transform.position)
        {
            destination++;
        }

        if (destination < MovePos.Length)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, MovePos[destination].transform.position, (Time.deltaTime * GlobalValue.Game_Speed) * MoveSpeed);
            this.transform.LookAt(MovePos[destination].transform);
        }

        else if (destination == MovePos.Length)
        {
            Destroy(this.gameObject);
            GlobalValue.Remain_Monster--;
            GlobalValue.MyLife--;
            GameObject go = Instantiate(Die_Ptc, this.transform.position + Vector3.up * 0.5f, this.transform.rotation);
            Destroy(go, 0.3f);
        }
    }

    void Dying()
    {
        if (Mon_HP <= 0)
        {
            int PlusGame = Random.Range(1, 101);
            if (PlusGame <= 3)
                GlobalValue.MyGem++;

            Destroy(this.gameObject);
            GlobalValue.Remain_Monster--;
            GlobalValue.MyGold += GlobalValue.Game_Stage;
        }   
    }

    void SettingHpBar()
    {
        Hp_Bar.fillAmount = Mon_HP / Max_HP;
    }
}