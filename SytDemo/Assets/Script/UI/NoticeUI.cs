using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using SytUI;

/// <summary>
/// 提示弹窗
/// </summary>
public class NoticeUI : UIbase
{
    private Text Title, Content;
    public NoticeUI() : base(UIType.PopUp, UIMode.DoNothing, UICollider.Normal)
    {
        uiPath = "UIPrefab/NoticeUI";
    }

    public override void Awake(GameObject go)
    {
        Title = transform.FindChildPlus("Title").GetComponent<Text>();
        Content = transform.FindChildPlus("Content").GetComponent<Text>();
        transform.FindChildPlus("Btn_Close").GetComponent<Button>().onClick.AddListener(() =>
        {
            ClosePage<NoticeUI>();
        });

        
    }
    public override void Active()
    {
        base.Active();

        //打开提示界面动画效果
        Transform panel = transform.Find("Panel");
        panel.localScale = Vector3.zero;
        panel.DOScale(Vector3.one,0.5f);
    }
    public override void Refresh()
    {
        base.Refresh();

        if(null != data)
        {
            Title.text = ((NoticeInfo)data).Title;
            Content.text = ((NoticeInfo)data).Content;
        }
    }
    public override void Hide()
    {
        base.Hide();
        Title.text = string.Empty;
        Content.text = string.Empty;
    }
}

/// <summary>
/// 提示弹窗信息(包括标题和内容)
/// </summary>
public class NoticeInfo
{
    public string Title;
    public string Content;
    public NoticeInfo(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
