using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SytUI;

/// <summary>
/// 综合操作UI
/// </summary>
public class OperateUI : UIbase
{
    public OperateUI() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/OperateUI";
    }

    public override void Awake(GameObject go)
    {
        transform.Find("Btn_Back").GetComponent<Button>().onClick.AddListener(() =>
        {
            ClosePage();
        });

        transform.Find("Btn_DeveloperInfo").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<NoticeUI>(new NoticeInfo("感谢使用","开发者:石艺田"));
        });

        transform.Find("Btn_Exit").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.LogError("退出游戏");
            //Application.Quit();
        });
    }
}
