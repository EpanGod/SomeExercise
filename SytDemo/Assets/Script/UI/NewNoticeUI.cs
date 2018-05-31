using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewNoticeUI : NewBase
{
    public string Title;
    public string Content;
    public Text TitleText;
    public Text ContentText;

    public override void Init()
    {
        base.Init();
        transform.FindChildPlus("Btn_Close").GetComponent<Button>().onClick.AddListener(()=> 
        {
            gameObject.SetActive(false);
        });
    }
    public override void Refresh()
    {
        base.Refresh();
        TitleText.text = Title;
        ContentText.text = Content;
    }
}
