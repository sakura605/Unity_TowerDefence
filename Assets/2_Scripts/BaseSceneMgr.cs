using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseSceneMgr : MonoBehaviour
{
    [SerializeField] GameObject LoadingWnd;
    static BaseSceneMgr Instance;

    public static BaseSceneMgr instance
    {
        get { return Instance; }
    }
    void Awake()
    {
        StartTitleScene();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadingScene(string remove, string load = "")
    {
        AsyncOperation AOper;
        GameObject go = Instantiate(LoadingWnd);

        if (remove != string.Empty)
        {
            AOper = SceneManager.UnloadSceneAsync(remove);
            while (!AOper.isDone)
            {
                go.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().fillAmount = AOper.progress;
                yield return null;
            }
        }

        if (Time.timeScale == 0)
            Time.timeScale = 1;

        yield return new WaitForSeconds(2.0f);
        AOper = SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive);
        while (!AOper.isDone)
        {
            go.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().fillAmount = AOper.progress;
            yield return null;
        }
        Destroy(go);
        StopAllCoroutines();
    }

    public void StartTitleScene()
    {
        SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
    }

    public void StartGameScene(string removeName = "")
    {
        StartCoroutine(LoadingScene(removeName, "GameScene"));
    }

    public void BackTitleScene(string removeName = "")
    {
        StartCoroutine(LoadingScene(removeName, "TitleScene"));
    }
}
