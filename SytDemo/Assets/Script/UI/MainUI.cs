using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SytUI;

/// <summary>
/// Main场景UI
/// </summary>
public class MainUI : MonoBehaviour
{
    private void Start()
    {
        UIbase.ShowPage<HeadUI>();
        UIbase.ShowPage<OperateUI>();
    }
}
