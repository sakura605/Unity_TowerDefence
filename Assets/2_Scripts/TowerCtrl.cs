using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCtrl : MonoBehaviour
{
    private int Tower_Lv;
    private int Atk_Dmg;
    private float Atk_Speed;
    private float Atk_Range;
    private string Skill_Name;
    private string Skill_Icon;
    private string Skill_Type;
    private string Skill_Ex;
    private bool BuildOn = false;

    [SerializeField] GameObject Knight_pro;
    [SerializeField] GameObject Lich_pro;
    [SerializeField] GameObject Ninja_pro;
    [SerializeField] GameObject Hit_Ptc;

    [HideInInspector] public GameObject Target = null;
    [HideInInspector] public TowerType TT;
    public int p_Lv { get { return Tower_Lv; } set { Tower_Lv = value; } }
    public int p_Dmg { get { return Atk_Dmg; } set { Atk_Dmg = value; } }
    public float p_Spd { get { return Atk_Speed; } set { Atk_Speed = value; } }
    public float p_Range { get { return Atk_Range; } set { Atk_Range = value; } }
    public bool p_Build { get { return BuildOn; } set { BuildOn = value; } }

    public string p_Sk_Name { get { return Skill_Name; } set { Skill_Name = value; } }
    public string p_Sk_Icon { get { return Skill_Icon; } set { Skill_Icon = value; } }
    public string p_Sk_Type { get { return Skill_Type; } set { Skill_Type = value; } }
    public string p_Sk_Ex { get { return Skill_Ex; } set { Skill_Ex = value; } }
    private Animator Tower_Anim;

    // Start is called before the first frame update
    void Start()
    {
        Tower_Anim = this.GetComponent<Animator>();
        Hit_Ptc = Resources.Load<GameObject>("Particlecollection_Free samples/Prefab/Hit/Hit_03");

        if (this.name.Contains("Lich"))
            SetTowerInfo(TowerType.Lich, GlobalValue.Lich_TW_LV, GlobalValue.Lich_TW_LV * 10, 2.0f, 5.0f,
                "Rage",
                "SpellBookPreface_png/SpellBookPreface_24",
                "<color=#FF0D00>액티브</color>",
                "지정한 타워의 공격력\n과 공격속도를\n상승시킨다.");

        else if (this.name.Contains("Knight"))
            SetTowerInfo(TowerType.Knight, GlobalValue.Knight_TW_LV, GlobalValue.Knight_TW_LV * 20, 1.0f, 2.5f,
                "Sword Rain",
                "SpellBookPreface_png/SpellBookPreface_03",
                "패시브",
                "공격시 " + GlobalValue.Knight_TW_LV + "프로 확률로\n맵의 모든 몬스터에게\n검을 내리 꽂는다.");
        else if (this.name.Contains("Junko"))
            SetTowerInfo(TowerType.Ninja, GlobalValue.Ninja_TW_LV, GlobalValue.Ninja_TW_LV * 20, 1.0f, 4.0f,
                "Sword Rain",
                "SpellBookPreface_png/SpellBookPreface_25",
                "패시브",
                "공격시" + GlobalValue.Ninja_TW_LV * 10 + "프로 확률로\n적에게 " + GlobalValue.Ninja_TW_LV * 2 + "배의 치명\n타 피해를입힙니다.");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.name.Contains("Lich"))
        {
            Tower_Lv = GlobalValue.Lich_TW_LV;
            Atk_Dmg = GlobalValue.Lich_TW_LV * 10;
        }
        else if (this.name.Contains("Knight"))
        {
            Tower_Lv = GlobalValue.Knight_TW_LV;
            Atk_Dmg = GlobalValue.Knight_TW_LV * 20;
            Skill_Ex = "공격시 " + GlobalValue.Knight_TW_LV + "프로 확률로\n맵의 모든 몬스터에게\n검을 내리 꽂는다.";
        }
        else if (this.name.Contains("Junko"))
        {
            Tower_Lv = GlobalValue.Ninja_TW_LV;
            Atk_Dmg = GlobalValue.Ninja_TW_LV * 20;
        }

        if (BuildOn)
        {
            Tower_Anim.speed = GlobalValue.Game_Speed;
            if (Target == null)
            {
                this.transform.eulerAngles = new Vector3(0, (Mathf.MoveTowards(this.transform.eulerAngles.y, 180.0f, (Time.deltaTime * GlobalValue.Game_Speed) * 90)), 0);
                UpdateTarget();
            }
            else
                this.transform.LookAt(Target.transform);
        }
    }

    void SetTowerInfo(TowerType a_TT, int Lv, int Dmg, float Spd, float Ran, 
        string sk_name, string sk_icon, string sk_type, string sk_ex)
    {
        TT = a_TT; Tower_Lv = Lv; Atk_Dmg = Dmg; Atk_Speed = Spd; Atk_Range = Ran;
        Skill_Name = sk_name; Skill_Icon = sk_icon; Skill_Type = sk_type; Skill_Ex = sk_ex;
    }

    void UpdateTarget()
    {
        GameObject[] Monsters = GameObject.FindGameObjectsWithTag("Monster"); //맵에있는 몬스터를 전부 가져온다.
        float shortestDistance = Mathf.Infinity;    //가장 짧은 거리
        GameObject nearestMonster = null;           //가장 가까운 몬스터

        foreach (GameObject Monster in Monsters)     //좀비의 수만큼 반복문을 돌린다.
        {
            float DistanceToMonsters = Vector3.Distance(this.transform.position, Monster.transform.position);
            //좀비와 플레이어간의 거리를 받아온다.

            if (DistanceToMonsters < shortestDistance)  //위에서 받아온거리가 전에 받아왔던 거리보다 짧다면
            {
                shortestDistance = DistanceToMonsters;  //거리를 새로 업데이트 해준 후
                nearestMonster = Monster;               //그 몬스터를 가장 가까운 적으로 인식시킨다.
            }
        }

        if (nearestMonster != null && shortestDistance <= Atk_Range)    //타겟팅할 적이 있고 거리가 사정거리 안쪽이라면
        {
            Target = nearestMonster;              //타겟팅할 적을 담아 준다.
            Tower_Anim.SetBool("IsAttack", true); //애니메이션도 공격모션으로
        }

        else  //타겟팅할 적도 없고 사정거리 바깥이라면
        {
            Target = null;  //타겟을 null로 초기화시킨다.
            Tower_Anim.SetBool("IsAttack", false);    //애니메이션도 Idle로 바꿔준다.
        }
    }

    public void InitProjectile()
    {
        if (Target == null)
            return;
        if (TT == TowerType.Knight)
        {
            int rand = Random.Range(0, 100);

            if (rand < GlobalValue.Knight_TW_LV)
            {
                MonsterCtrl[] MC = FindObjectsOfType<MonsterCtrl>();               
                for (int i = 0; i < MC.Length; i++)
                {
                    Vector3 ObsPos = MC[i].transform.position + Vector3.up * 2.0f;
                    GameObject go = Instantiate(Knight_pro, ObsPos, Knight_pro.transform.rotation);
                    go.GetComponent<FollowTarget>().Target = MC[i].transform;
                    Destroy(go, 1.5f);
                }
            }

            else
            {
                Vector3 ObsPos = Target.transform.position + Vector3.up * 2.0f;
                GameObject go = Instantiate(Knight_pro, ObsPos, Knight_pro.transform.rotation);
                go.GetComponent<FollowTarget>().Target = Target.transform;
                Destroy(go, 1.5f);
            }

        }

        else if (TT == TowerType.Lich)
        {
            Vector3 ObsPos = Target.transform.position + Vector3.up * 2.0f;
            GameObject go = Instantiate(Lich_pro, ObsPos, Lich_pro.transform.rotation);
            go.GetComponent<FollowTarget>().Target = Target.transform;
            Destroy(go, 1.5f);
        }

        else if (TT == TowerType.Ninja)
        {
            Vector3 ObsPos = this.transform.position + Vector3.up * 1.0f;
            Instantiate(Ninja_pro, ObsPos, Ninja_pro.transform.rotation, this.transform);
        }
    }

    public void TakeDamage()
    {
        if (Target == null)
            return;

        int Cri = Random.Range(1, 101);
        if ((GlobalValue.Ninja_TW_LV * 10) < Cri)
        {
            Target.GetComponent<MonsterCtrl>().Mon_HP -= Atk_Dmg;
        }
        else
        {
            Target.GetComponent<MonsterCtrl>().Mon_HP -= Atk_Dmg * (GlobalValue.Ninja_TW_LV * 2);
        }
        this.GetComponent<AudioSource>().time = 0.25f;
        this.GetComponent<AudioSource>().Play();
        GameObject go = Instantiate(Hit_Ptc, Target.transform.position + Vector3.up * 0.5f, Target.transform.rotation);
        Destroy(go, 0.3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Atk_Range);
    }
}
