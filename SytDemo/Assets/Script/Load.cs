using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class Load : MonoBehaviour
{
    public Button GameStart, Mask;
    public Slider LoadProgress;
    public Text LoadingNum;

    private const string scenename = "Main";

    //异步对象  
    AsyncOperation asyncOperation;
    void Start()
    {
        Mask.gameObject.SetActive(false);

        GameStart.onClick.AddListener(() =>
        {
            Mask.gameObject.SetActive(true);

            InitAll.Init();
            StartCoroutine(StartLoading(scenename));
        });
    }


    /// <summary>
    /// 设置进度条百分比
    /// </summary>
    /// <param name="displayProgress">百分比数值</param>
    private void SetLoadingPercentage(int displayProgress)
    {
        LoadProgress.value = (float)displayProgress / 100.0f;
        LoadingNum.text = displayProgress.ToString() + "%";
    }

    /// <summary>
    /// 加载场景协程
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <returns></returns>
    private IEnumerator StartLoading(string sceneName)
    {
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        while(op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while(displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        while(displayProgress < toProgress)
        {
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }

}
