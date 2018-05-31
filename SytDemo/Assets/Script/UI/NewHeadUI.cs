using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewHeadUI : NewBase {

    public override void Init()
    {
        base.Init();
        GetComponent<Button>().onClick.AddListener(()=> 
        {
            NewMainUI.Instance.UIdic["AttributeUI"].SetActive(true);
        });
    }
}
