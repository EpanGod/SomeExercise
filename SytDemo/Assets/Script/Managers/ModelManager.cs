using UnityEngine;
using System.Collections;

/// <summary>
/// Model单例管理类
/// </summary>
public class ModelManager : MonoSingleton<ModelManager>
{
    public override void Init()
    {
        base.Init();
        Debug.Log("初始化模型管理");
    }
}
