using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SytUI;

/// <summary>
/// 角色头像
/// </summary>
public class HeadUI : UIbase
{
    public HeadUI() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/HeadUI";
    }

    public override void Awake(GameObject go)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<AttributeUI>();
        });
    }
}
