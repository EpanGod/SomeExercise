using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewBagUI : NewBase
{
    private UIWarpContent warpContent;
    private GameObject warpContentItem;

    public override void Init()
    {
        base.Init();
        transform.Find("Btn_Close").GetComponent<Button>().onClick.AddListener(()=> 
        {
            gameObject.SetActive(false);
        });      
    }

    public override void OnStart()
    {
        base.OnStart();
        //滑动列表相关配置
        TempAddBagItem(20);//给背包添加假数据
        warpContentItem = Resources.Load<GameObject>("UIPrefab/WarpContentItem");
        if(null == warpContentItem)
        {
            Debug.LogError("背包内item预制体不存在或路径错误!!");
            return;
        }
        warpContent = transform.FindChildPlus("Scroll View").GetComponent<UIWarpContent>();
        #region 代码配置滑动列表
        if(null == warpContent)
        {
            warpContent = transform.FindChildPlus("Scroll View").gameObject.AddComponent<UIWarpContent>();
            warpContent.arrangement = UIWarpContent.Arrangement.Vertical;
            warpContent.maxPerLine = 1;
            warpContent.cellHeight = 100;
            warpContent.cellWidth = 160;
            warpContent.cellHeightSpace = 10;
            warpContent.scrollRect = transform.FindChildPlus("Scroll View").GetComponent<ScrollRect>();
            warpContent.content = transform.FindChildPlus("Content").GetComponent<RectTransform>();
            warpContent.goItemPrefab = warpContentItem;
        }
        #endregion
        warpContent.onInitializeItem = onInitializeItem;
        warpContent.Init(20);
    }

    private void onInitializeItem(GameObject go, int index)
    {
        Item tempItem = BagItemData.GetItem(index);
        go.transform.Find("Name").GetComponent<Text>().text = tempItem.Name + ":" + index;

        Button tempInformationButton = go.transform.Find("Information").GetComponent<Button>();
        tempInformationButton.onClick.RemoveAllListeners();
        tempInformationButton.onClick.AddListener(() =>
        {
            //ShowPage<NoticeUI>(new NoticeInfo("详细信息", tempItem.Name + tempItem.Index));
            NewMainUI.Instance.OpenNotice("详细信息", tempItem.Name + tempItem.Index);
        });
    }

    /// <summary>
    /// 临时添加背包物品数据
    /// </summary>
    private void TempAddBagItem(int num)
    {
        for(int i = 0; i < num; i++)
        {
            BagItemData.AddItem(new Item("物品", i));
        }
    }
}
