using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewOperateUI : NewBase {

    public override void Init()
    {
        base.Init();
        transform.Find("Btn_DeveloperInfo").GetComponent<Button>().onClick.AddListener(()=> 
        {
            NewMainUI.Instance.OpenNotice("感谢使用", "开发者:石艺田");
        });
        transform.Find("Btn_Exit").GetComponent<Button>().onClick.AddListener(()=> 
        {
            Debug.LogError("退出游戏");
            //Application.Quit();
        });
    }
}
