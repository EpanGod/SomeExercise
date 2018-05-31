using UnityEngine;
using System.Collections;

/// <summary>
/// Transform助手
/// </summary>
public static class TransformHelper
{
    /// <summary>
    /// 转向
    /// </summary>
    public static void LookAtTarget(this Transform pTransform, Vector3 targetDir, float rotationSpeed)
    {
        if (targetDir != Vector3.zero)
        {
            Quaternion dir = Quaternion.LookRotation(targetDir);
            pTransform.rotation = Quaternion.Lerp(pTransform.rotation, dir, rotationSpeed);
        }
    }

    /// <summary>
    /// 在所有子级中查找子物体
    /// </summary>
    /// <returns></returns>
    public static Transform FindChildPlus(this Transform pTransform, string goName)
    {
        var child = pTransform.FindChild(goName);
        if (child != null) return child;
        for (int i = 0; i < pTransform.childCount; i++)
        {
            child = pTransform.GetChild(i);
            var go = FindChildPlus(child, goName);
            if (go != null)
                return go;
        }
        return null;
    }

    public static void SetTransform(this Transform pTransform, Transform pParent, Vector3 pLocalPosition, Quaternion pLocalRotation)
    {
        pTransform.parent = pParent;
        pTransform.localPosition = pLocalPosition;
        pTransform.localRotation = pLocalRotation;
    }
}
