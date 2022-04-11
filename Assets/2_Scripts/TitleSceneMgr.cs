using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneMgr : MonoBehaviour
{
    [SerializeField] Image Title_Img;
    [SerializeField] Image Fade_Panel;
    [SerializeField] Button Start_Btn;
    [SerializeField] Button Exit_Btn;
    // Start is called before the first frame update
    void Start()
    {
        Title_Img.transform.localPosition = new Vector3(0, 1000, 0);
        StartCoroutine(FadeIn(Fade_Panel.color));

        Start_Btn.onClick.AddListener(() => { BaseSceneMgr.instance.StartGameScene("TitleScene"); Debug.Log(11); });
#if UNITY_EDITOR
        Exit_Btn.onClick.AddListener(() => { UnityEditor.EditorApplication.isPlaying = false; });
#endif
        Exit_Btn.onClick.AddListener(() => { Application.Quit(); });

    }

    // Update is called once per frame
    void Update()
    {
        Title_Img.transform.localPosition = Vector3.MoveTowards(Title_Img.transform.localPosition, new Vector3(0, 100, 0), Time.deltaTime * 500);
        Start_Btn.transform.localPosition = Vector3.MoveTowards(Start_Btn.transform.localPosition, new Vector3(0, -300, 0), Time.deltaTime * 800);
        Exit_Btn.transform.localPosition = Vector3.MoveTowards(Exit_Btn.transform.localPosition, new Vector3(0, -430, 0), Time.deltaTime * 800);
    }

    IEnumerator FadeIn(Color p_color)
    {
        Color color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        while (color.a > 0.0f)
        {
            Fade_Panel.color = color;
            color.a -= 0.01f;
            yield return null;
        }
    }
}
