using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMgr : MonoBehaviour
{
    [SerializeField] GameObject Apply_Skill_Mask;
    public static bool IsMask = false;
    [SerializeField] Image Apply_Skill_Img;
    [SerializeField] Button Poison_Fog_Btn;
    public static bool IsPoison = false;
    float tick = 3.0f;

    [SerializeField] Button Acid_Rain_Btn;
    public static bool IsRain = false;

    [SerializeField] GameObject Rain_Sword_Efc;
    GameInfoCanvasMgr GICM;

    // Start is called before the first frame update
    void Start()
    {
        GICM = FindObjectOfType<GameInfoCanvasMgr>();
        Poison_Fog_Btn.onClick.AddListener(()=> 
        {
            if (IsRain)
            {
                GICM.InitSkillSystemMsg();
                return;
            }
            else
            {
                if (GlobalValue.MyGem >= 1)
                {
                    IsPoison = true;
                    IsMask = true;
                    GlobalValue.MyGem--;
                    Apply_Skill_Img.sprite = Resources.Load<Sprite>("SpellBookPreface_png/SpellBookPreface_13");
                }
                else
                {
                    GICM.InitSystemMsg("<color=#8429C7>Gem</color>");
                }
            }
        });

        Acid_Rain_Btn.onClick.AddListener(() => 
        {
            if (IsPoison)
            {
                GICM.InitSkillSystemMsg();
                return;
            }
            else
            {
                if (GlobalValue.MyGem >= 1)
                {
                    IsRain = true;
                    IsMask = true;
                    GlobalValue.MyGem--;
                    Apply_Skill_Img.sprite = Resources.Load<Sprite>("SpellBookPreface_png/SpellBookPreface_05");
                }
                else
                {
                    GICM.InitSystemMsg("<color=#8429C7>Gem</color>");
                }
            }
        });
    }

    void Update()
    {
        if (IsPoison) Skill_PoisonFog();
        else tick = 3.0f;

        this.transform.GetChild(1).gameObject.SetActive(IsRain);
        this.transform.GetChild(0).gameObject.SetActive(IsPoison);
        Apply_Skill_Mask.SetActive(IsMask);
    }

    void Skill_PoisonFog()
    {
        tick -= Time.deltaTime * GlobalValue.Game_Speed;

        if (tick <= 0.0f)
        {
            tick = 3.0f;
            MonsterCtrl[] MC = FindObjectsOfType<MonsterCtrl>();
            for (int i = 0; i < MC.Length; i++)
            {
                MC[i].Mon_HP -= MonsterCtrl.Max_HP / 10;
            }
        }
    }
}
