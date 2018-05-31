using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewAttributeUI : NewBase {

    public override void Init()
    {
        base.Init();
        transform.Find("Btn_Close").GetComponent<Button>().onClick.AddListener(()=> 
        {
            NewMainUI.Instance.UIdic["AttributeUI"].SetActive(false);
        });
        transform.Find("Btn_OpenBag").GetComponent<Button>().onClick.AddListener(()=> 
        {
            NewMainUI.Instance.UIdic["BagUI"].SetActive(true);
        });
    }
}
