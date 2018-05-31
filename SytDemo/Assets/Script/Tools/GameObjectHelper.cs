using UnityEngine;
using System.Collections;

/// <summary>
/// GameObject助手
/// </summary>
public static class GameObjectHelper
{
    public static GameObject CreateGameObject(GameObject pGameObject, Transform pParent, Vector3 pLocalPosition, Quaternion pLocalRotation, string pGameObjectName = "")
    {
        GameObject tempGameObject = null;
        if (pGameObject == null)
        {
            tempGameObject = new GameObject();
        }
        else
        {
            tempGameObject = GameObject.Instantiate(pGameObject, Vector3.zero, Quaternion.identity) as GameObject;
        }

        tempGameObject.transform.SetParent(pParent);
        tempGameObject.transform.localPosition = pLocalPosition;
        tempGameObject.transform.localRotation = pLocalRotation;
        if (pGameObjectName != "")
        {
            tempGameObject.name = pGameObjectName;
        }
        return tempGameObject;
    }
}