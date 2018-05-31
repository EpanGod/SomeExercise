using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SytUI;

/// <summary>
/// 角色属性UI
/// </summary>
public class AttributeUI : UIbase
{
    public AttributeUI() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/AttributeUI";
    }

    public override void Awake(GameObject go)
    {
        transform.Find("Btn_Close").GetComponent<Button>().onClick.AddListener(() =>
        {
            ClosePage<AttributeUI>();
        });

        transform.Find("Btn_OpenBag").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<BagUI>();
        });
    }
}
