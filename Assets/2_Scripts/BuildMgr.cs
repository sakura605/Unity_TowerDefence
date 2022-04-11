using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMgr : MonoBehaviour
{
    TowerType TT;
    [SerializeField] Button Random_Build_Btn;
    [SerializeField] Button Lich_Build_Btn;
    [SerializeField] Button Knight_Build_Btn;
    [SerializeField] Button Ninja_Build_Btn;

    [SerializeField] GameObject Lich_Tower;
    [SerializeField] GameObject Knight_Tower;
    [SerializeField] GameObject Ninja_Tower;
    GameObject[] Tower;

    [SerializeField] Button Tower_Sell_Btn;
    GameObject Sel_Sell_Tower;

    private int Rand_Price = 10;
    private int Lich_Price = 20;
    private int Knight_Price = 20;
    private int Ninja_Price = 100;

    bool Build_Mode = false;
    bool Select_Tower = false;

    GameInfoCanvasMgr GICM;
    // Start is called before the first frame update
    void Start()
    {
        GICM = FindObjectOfType<GameInfoCanvasMgr>();

        Random_Build_Btn.onClick.AddListener(() =>
        {
            if (GlobalValue.MyGold >= Rand_Price)
            {
                Build_Mode = !Build_Mode;
                TT = TowerType.Random;
            }
            else
                GICM.InitSystemMsg("<color=#C6F300>Gold</color>");
        });

        Lich_Build_Btn.onClick.AddListener(() => 
        {
            if (GlobalValue.MyGold >= Lich_Price)
            {
                Build_Mode = !Build_Mode;
                TT = TowerType.Lich;
            }
            else
                GICM.InitSystemMsg("<color=#C6F300>Gold</color>");
        });
        Knight_Build_Btn.onClick.AddListener(() => 
        {
            if (GlobalValue.MyGold >= Knight_Price)
            {
                Build_Mode = !Build_Mode;
                TT = TowerType.Knight;
            }
            else
                GICM.InitSystemMsg("<color=#C6F300>Gold</color>");
        });
        Ninja_Build_Btn.onClick.AddListener(() => 
        {
            if (GlobalValue.MyGold >= Ninja_Price)
            {
                Build_Mode = !Build_Mode;
                TT = TowerType.Ninja;
            }
            else
                GICM.InitSystemMsg("<color=#C6F300>Gold</color>");
        });


        Tower_Sell_Btn.onClick.AddListener(() =>
        {
            Destroy(Sel_Sell_Tower);
            if (Sel_Sell_Tower.name.Contains("Lich"))
                GlobalValue.MyGold += Lich_Price / 2;
            else if (Sel_Sell_Tower.name.Contains("Knight"))
                GlobalValue.MyGold += Knight_Price / 2;
            else if (Sel_Sell_Tower.name.Contains("Ninja"))
                GlobalValue.MyGold += Ninja_Price / 2;
        });
    }

    // Update is called once per frame
    void Update()
    {
        Tower_Sell_Btn.gameObject.SetActive(Select_Tower);

        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        RaycastHit hitInfo;

        if (Build_Mode)
        {
            Tower = GameObject.FindGameObjectsWithTag("TowerPos");
            for (int i = 0; i < Tower.Length; i++)
            {
                if (Tower[i].transform.childCount == 0)
                    Tower[i].GetComponent<MeshRenderer>().material.color = Color.yellow;
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(t_MousePos), out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.transform.CompareTag("TowerPos") && hitInfo.transform.childCount == 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (TT == TowerType.Random)
                        {
                            GameObject go;
                            int a = Random.Range(1, 101);
                            if (a <= 45) go = Lich_Tower;
                            else if (a <= 90) go = Knight_Tower;
                            else go = Ninja_Tower;
                            StartCoroutine(Tower_Building(go, hitInfo.transform));
                            GlobalValue.MyGold -= Rand_Price;
                        }
                        else if (TT == TowerType.Lich)
                        {
                            StartCoroutine(Tower_Building(Lich_Tower, hitInfo.transform));
                            GlobalValue.MyGold -= Lich_Price;
                        }
                        else if (TT == TowerType.Knight)
                        {
                            StartCoroutine(Tower_Building(Knight_Tower, hitInfo.transform));
                            GlobalValue.MyGold -= Knight_Price;
                        }
                        else if (TT == TowerType.Ninja)
                        {
                            StartCoroutine(Tower_Building(Ninja_Tower, hitInfo.transform));
                            GlobalValue.MyGold -= Ninja_Price;
                        }
                        Build_Mode = false;
                    }

                    else if (Input.GetMouseButtonDown(1))
                    {
                        Build_Mode = false;
                    }
                }
            }
        }

        else
        {
            Tower = GameObject.FindGameObjectsWithTag("TowerPos");
            for (int i = 0; i < Tower.Length; i++)
                Tower[i].GetComponent<MeshRenderer>().material.color = Color.white;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(t_MousePos), out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.transform.CompareTag("Tower"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Select_Tower = true;
                        TowerCtrl TC = hitInfo.transform.GetComponent<TowerCtrl>();
                        GlobalValue.Tower_Class = TC.TT.ToString();
                        GlobalValue.Tower_Level = TC.p_Lv;
                        GlobalValue.Tower_AtkDmg = TC.p_Dmg;
                        GlobalValue.Tower_AtkSpd = TC.p_Spd;
                        GlobalValue.Tower_AtkRange = TC.p_Range;
                        GlobalValue.Tower_Skill_Name = TC.p_Sk_Name;
                        GlobalValue.Tower_Skill_Image_Name = TC.p_Sk_Icon;
                        GlobalValue.Tower_Skill_Type = TC.p_Sk_Type;
                        GlobalValue.Tower_Skill_Ex = TC.p_Sk_Ex;
                        Sel_Sell_Tower = hitInfo.transform.gameObject;
                    }
                }
            }
        }
    }
    IEnumerator Tower_Building(GameObject Tower, Transform Tf)
    {
        Vector3 StartPos = Tf.position + (Vector3.up * -1.0f);
        Vector3 EndPos = Tf.position + (Vector3.up * 0.5f);
        float dist = Vector3.Distance(StartPos, EndPos);

        Tower = Instantiate(Tower, StartPos, Tower.transform.rotation);
        Image Img = Tower.GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<Image>();

        while (Vector3.Distance(Tower.transform.position, EndPos) != 0)
        {
            StartPos = Vector3.MoveTowards(StartPos, EndPos, Time.deltaTime * GlobalValue.Game_Speed * 0.3f);
            Tower.transform.position = StartPos;
            Img.fillAmount = (dist - Vector3.Distance(StartPos, EndPos)) / dist;
            if (Img.fillAmount >= 1.0f)
                Img.GetComponentInParent<Canvas>().gameObject.SetActive(false);
            yield return null;
        }

        Tower.GetComponent<TowerCtrl>().p_Build = true;
    }
}