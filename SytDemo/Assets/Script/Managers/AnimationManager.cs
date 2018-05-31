using UnityEngine;
using System.Collections;

/// <summary>
/// UI管理单例类
/// </summary>
public class AnimationManager : MonoSingleton<AnimationManager>
{
    public override void Init()
    {
        base.Init();
        Debug.Log("初始化动画管理");
    }
}
