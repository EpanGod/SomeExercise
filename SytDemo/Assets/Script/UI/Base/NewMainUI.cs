using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewMainUI : MonoBehaviour
{
    public List<GameObject> UIs = new List<GameObject>();
    public static NewMainUI Instance;
    private Dictionary<string, GameObject> uidic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> UIdic
    {
        get { return uidic; }
    }

    private void Awake()
    {
        Instance = this;
        for(int i = 0; i < UIs.Count; i++)
        {
            uidic.Add(UIs[i].name, UIs[i]);
            UIs[i].SetActive(false);
        }
    }

    private void Start()
    {
        UIdic["HeadUI"].SetActive(true);
        UIdic["OperateUI"].SetActive(true);
    }

    public void OpenNotice(string title,string content)
    {
        NewNoticeUI tempNotice = UIdic["NoticeUI"].GetComponent<NewNoticeUI>();
        tempNotice.Title = title;
        tempNotice.Content = content;
        UIdic["NoticeUI"].SetActive(true);
    }
}
