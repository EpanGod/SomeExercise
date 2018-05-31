using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SytUI;

public class DeveloperInfoUI : UIbase
{
    public DeveloperInfoUI() : base(UIType.PopUp, UIMode.DoNothing, UICollider.Normal)
    {
        uiPath = "UIPrefab/DeveloperInfoUI";
    }

    public override void Awake(GameObject go)
    {
        transform.FindChildPlus("Btn_Close").GetComponent<Button>().onClick.AddListener(() =>
        {
            ClosePage<DeveloperInfoUI>();
        });
    }
}
