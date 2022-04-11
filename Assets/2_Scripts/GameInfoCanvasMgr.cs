using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoCanvasMgr : MonoBehaviour
{
    [SerializeField] GameObject SystemMsg;
    [SerializeField] Image GameOver_Img;

    [SerializeField] Button Skip_Btn;
    [SerializeField] Button GameSpeed_Btn;
    [SerializeField] Button Pause_Btn;

    [SerializeField] Button Tower_Build_Tab;
    [SerializeField] Button Tower_Upgrade_Tab;
    [SerializeField] Button Tower_Skill_Tab;

    [SerializeField] Button Retry_Btn;
    [SerializeField] Button Exit_Btn;
    bool TabOnOff = false;
    bool IsFinish = true;
    Vector3 StartPos;
    Vector3 EndPos;

    [SerializeField] Text Stage_Txt;
    [SerializeField] Text MyGold_Txt;
    [SerializeField] Text MyLife_Txt;
    [SerializeField] Text MyGem_Txt;
    [SerializeField] Text StageStart_Txt;

    [SerializeField] Text Remain_Wave_Time;
    [SerializeField] Text Remain_Mon_Cnt;
    float WaveTime;
    float MaxWaveTime = 120;

    bool BuildMode = true;
    bool StageStart = false;

    // Start is called before the first frame update
    void Start()
    {
        WaveTime = 60;

        Skip_Btn.onClick.AddListener(() => 
        {
            if (!SpawnMgr.Stage_Start)
                StartCoroutine(StageStartCo());
        });

        GameSpeed_Btn.onClick.AddListener(() =>
        {
            GlobalValue.Game_Speed++;
            if (GlobalValue.Game_Speed > 3)
                GlobalValue.Game_Speed = 1;
        });

        Pause_Btn.onClick.AddListener(() =>
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        });

        Tower_Build_Tab.onClick.AddListener(() =>
        {
            if (IsFinish) StartCoroutine(TabCtrlCo(Tower_Build_Tab));
        });

        Tower_Upgrade_Tab.onClick.AddListener(() =>
        {
            if (IsFinish) StartCoroutine(TabCtrlCo(Tower_Upgrade_Tab));
        });

        Tower_Skill_Tab.onClick.AddListener(() =>
        {
            if (IsFinish) StartCoroutine(TabCtrlCo(Tower_Skill_Tab));
        });

        Retry_Btn.onClick.AddListener(() =>
        {
            BaseSceneMgr.instance.StartGameScene("GameScene");
        });

        Exit_Btn.onClick.AddListener(() =>
        {
            BaseSceneMgr.instance.BackTitleScene("GameScene");
        });
    }

    float deltaTime;
    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        ShowGameSpeed();
        ShowWaveTime();
        ShowGameInfo();
        ShowRemainMonCnt();
        EndGame();
    }

    void ShowGameSpeed()
    {
        GameSpeed_Btn.GetComponentInChildren<Text>().text = "x" + GlobalValue.Game_Speed;
    }

    void ShowWaveTime()
    {
        WaveTime -= Time.deltaTime * GlobalValue.Game_Speed;
        Remain_Wave_Time.text = "0" + ((int)(WaveTime / 60)).ToString("N0") + " : " + ((int)(WaveTime % 60)).ToString("N0");
        if (((int)(WaveTime % 60)) < 10)
            Remain_Wave_Time.text = "0" + ((int)(WaveTime / 60)).ToString("N0") + " : 0" + ((int)(WaveTime % 60)).ToString("N0");

        if (WaveTime < 0)
        {
            if (!StageStart)
                StartCoroutine(StageStartCo());
        }
    }

    void ShowGameInfo()
    {
        Stage_Txt.text = "Stage " + GlobalValue.Game_Stage;
        MyGold_Txt.text = GlobalValue.MyGold.ToString();
        MyLife_Txt.text = GlobalValue.MyLife.ToString();
        MyGem_Txt.text = GlobalValue.MyGem.ToString();
    }

    void ShowRemainMonCnt()
    {
        Remain_Mon_Cnt.text = GlobalValue.Remain_Monster + "마리";
    }

    void SetNextStage()
    {
        SpawnMgr.Stage_Start = true;
        WaveTime = MaxWaveTime;
        GlobalValue.Game_Stage++;
        GlobalValue.Spawn_Mon_Cnt = 0;
        GlobalValue.Remain_Monster += 20;
    }

    void EndGame()
    {
        if (GlobalValue.MyLife <= 0 && !GlobalValue.Game_End)
        {
            StartCoroutine(GameOverCo());
        }
    }

    IEnumerator StageStartCo()
    {
        float CheckTime = 1.0f;
        Vector3 StartSize = new Vector3(5, 5, 1);
        Vector3 EndSize = Vector3.one;

        StageStart = true;
        StageStart_Txt.enabled = true;
        StageStart_Txt.text = 3.ToString();

        SkillMgr.IsPoison = false;
        SkillMgr.IsRain = false;
        SkillMgr.IsMask = false;

        for (int i = 3; i > 0;)
        {
            while (StageStart)
            {
                CheckTime -= Time.deltaTime * GlobalValue.Game_Speed;
                StageStart_Txt.transform.localScale = StartSize;
                StartSize = Vector3.MoveTowards(StartSize, EndSize, (Time.deltaTime * GlobalValue.Game_Speed * 8));
                if (i != 0)
                {
                    StageStart_Txt.text = i.ToString();
                    if (CheckTime <= 0.0f)
                    {
                        i--;
                        StartSize = new Vector3(5, 5, 1);
                        CheckTime = 1.0f;
                    }
                    yield return null;
                }

                else
                {
                    StageStart_Txt.text = "START!";
                    if (CheckTime <= 0.0f)
                    {
                        yield return new WaitForSeconds(1.0f / (float)GlobalValue.Game_Speed);
                        StartSize = new Vector3(5, 5, 1);
                        StageStart_Txt.enabled = false;
                        SetNextStage();
                        CheckTime = 1.0f;
                        StageStart = false;
                    }
                    yield return null;
                }
            }
        }
    }

    IEnumerator GameOverCo()
    {
        GlobalValue.Game_End = true;
        bool isFinish = false;

        GameOver_Img.enabled = true;
        GameOver_Img.transform.localScale = new Vector3(10, 10, 1);

        while (!isFinish)
        {
            GameOver_Img.transform.localScale = Vector3.MoveTowards(GameOver_Img.transform.localScale, Vector3.one, Time.deltaTime * 20);
            GameOver_Img.transform.Rotate(Vector3.forward * 1800 * Time.deltaTime);

            if (GameOver_Img.transform.localScale == Vector3.one)
            {
                Time.timeScale = 0;
                for (int i = 0; i < GameOver_Img.transform.childCount; i++)
                {
                    GameOver_Img.transform.GetChild(i).gameObject.SetActive(true);
                }
                isFinish = true;
            }
            yield return null;
        }

        GameOver_Img.transform.eulerAngles = new Vector3(0, 0, 15);
    }

    IEnumerator TabCtrlCo(Button Tab)
    {
        Tab.transform.SetAsLastSibling();
        Vector3 TargetPos;
        TabOnOff = !TabOnOff;
        IsFinish = false;

        if (TabOnOff)
        {
            EndPos = Tab.transform.localPosition + Vector3.right * -450.0f;
            TargetPos = EndPos;
        }

        else
        {
            StartPos = Tab.transform.localPosition + Vector3.right * 450.0f;
            TargetPos = StartPos;
        }

        while (!IsFinish)
        {
            if (Vector3.Distance(TargetPos, Tab.transform.localPosition) == 0) IsFinish = true;
            if (TabOnOff) Tab.transform.localPosition = Vector3.MoveTowards(Tab.transform.localPosition, EndPos, Time.deltaTime * 1350.0f);
            else Tab.transform.localPosition = Vector3.MoveTowards(Tab.transform.localPosition, StartPos, Time.deltaTime * 1350.0f);
            yield return null;
        }
    }

    public void InitSystemMsg(string Msg)
    {
        GameObject go = Instantiate(SystemMsg, this.transform);
        if (Msg == "Gem")
            go.GetComponent<Text>().color = new Color32(133, 41, 201, 255);
        else if (Msg == "Gold")
            go.GetComponent<Text>().color = new Color32(198, 243, 0, 255);

        go.GetComponent<Text>().text = Msg + "이(가) 부족합니다.";
    }

    public void InitSkillSystemMsg()
    {
        GameObject go = Instantiate(SystemMsg, this.transform);

        if (SkillMgr.IsPoison)
            go.GetComponent<Text>().text = "이미 <color=#00ff00>독안개</color>효과가 적용중입니다.";
        else if (SkillMgr.IsRain)
            go.GetComponent<Text>().text = "이미 <color=#67D7FF>산성비</color>효과가 적용중입니다.";
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}