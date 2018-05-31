using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// UI管理单例类
/// </summary>
public class UIManager : MonoSingleton<UIManager>
{
    public Transform Root;
    public Camera UiCamera;
    public Transform NormalRoot, FixedRoot, PopupRoot;

    public override void Init()
    {
        base.Init();

        gameObject.name = "UIManager";
        gameObject.layer = LayerMask.NameToLayer("UI");
        gameObject.AddComponent<RectTransform>();

        Canvas can = gameObject.AddComponent<Canvas>();
        can.renderMode = RenderMode.ScreenSpaceCamera;
        can.pixelPerfect = true;

        gameObject.AddComponent<GraphicRaycaster>();

        Root = gameObject.transform;

        
        UiCamera = CreatUiCamera("UICamera",transform, new Vector3(0, 0, -100));
        can.worldCamera = UiCamera;

        NormalRoot = CreateSubCanvasForRoot("NormalRoot", transform).transform;
        FixedRoot = CreateSubCanvasForRoot("FixedRoot",transform).transform;
        PopupRoot = CreateSubCanvasForRoot("PopupRoot", transform).transform;
    }

    /// <summary>
    /// 创建UI相机
    /// </summary>
    /// <param name="name">相机名字</param>
    /// <param name="root">父节点</param>
    /// <param name="pos">本地位置</param>
    /// <returns></returns>
    private Camera CreatUiCamera(string name,Transform root,Vector3 localPos)
    {
        GameObject go = GameObjectPool.instance.CreateObject(name, null, root, localPos, Quaternion.identity);
        go.name = name;
        go.layer = LayerMask.NameToLayer("UI");
        Camera cam = go.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Depth;
        cam.orthographic = true;
        cam.farClipPlane = 200f;
        cam.cullingMask = 1 << 5;
        cam.nearClipPlane = -50f;
        cam.farClipPlane = 50f;
        return cam;
    }

    /// <summary>
    /// 创建UI层级
    /// </summary>
    /// <param name="root"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    private Transform CreateSubCanvasForRoot(string name, Transform root)
    {
        GameObject go = new GameObject("canvas");
        go.name = name;
        go.transform.parent = root;
        go.transform.localScale = Vector3.one;
        go.layer = LayerMask.NameToLayer("UI");

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        return go.transform;
    }
}
