using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

/// <summary>
/// 游戏物体对象池  只创建一个对象(单例)
/// </summary>
public class GameObjectPool : MonoSingleton<GameObjectPool>
{

    //缓存容器
    private Dictionary<string, List<GameObject>> cache = new Dictionary<string, List<GameObject>>();
    /// <summary>
    /// 创建对象 有缓存：从池中返回，无缓存：加载后再返回
    /// </summary>
    /// <returns></returns>

    /**************************************** 高层 ************************************************/
    public GameObject CreateObject(string pKey, GameObject pGameObject, Transform pParent, Vector3 pLocalPostion, Quaternion pLocalRotation, string pGameObjectName = "")
    {
        GameObject tempGO = FindUsable(pKey);
        //有缓存（没有出现在画面中的对象）：从池中返回，
        if (tempGO != null)
        {
            tempGO = CreateLocalExitObject(pKey, pParent, pLocalPostion, pLocalRotation, pGameObjectName);
        }
        else //无缓存：创建后再返回
        {
            tempGO = GameObjectHelper.CreateGameObject(pGameObject, pParent, pLocalPostion, pLocalRotation, pGameObjectName);

            Add(pKey, tempGO);
        }

        return tempGO;
    }


    /// <summary>全部释放资源 </summary>
    public void ClearAll()
    {
        //多次调用Clear(key)
        var list = new List<string>(cache.Keys);
        while (list.Count > 0)
        {
            Clear(list[0]);
            list.RemoveAt(0);
        }
    }
    /// <summary>按Key部分释放 </summary>
    public void Clear(string key)
    {
        if (cache.ContainsKey(key))
        {
            while (cache[key].Count > 0)
            {
                //执行多次释放GameObject，并删除对象对应的引用
                Destroy(cache[key][0]);
                cache[key].RemoveAt(0);
            }
            //删除key
            cache.Remove(key);
        }
    }
    /// <summary>即时回收对象 </summary>
    public void CollectObject(GameObject go)
    {
        //从画面中消失，回收到对象池
        go.transform.parent = null;
        go.SetActive(false);
    }
    /// <summary>延时回收对象</summary>
    public void CollectObject(GameObject go, float delay)
    {
        //启动协程做延时处理
        StartCoroutine(DelayCollect(go, delay));
    }
    /**********************************************************************************************/

    /**************************************** 低层 ************************************************/
    protected GameObject CreateLocalExitObject(string key, Transform pParent, Vector3 pLocalPostion, Quaternion pLocalRotation, string pGameObjectName = "")
    {
        var tempGo = FindUsable(key);

        if (tempGo == null) { throw new NullReferenceException("tempGO is null"); }
        tempGo.transform.SetTransform(pParent, pLocalPostion, pLocalRotation);
        if (pGameObjectName != "")
        {
            tempGo.name = pGameObjectName;
        }
        tempGo.SetActive(true);

        return tempGo;
    }

    /// <summary>
    /// 查找可用的对象
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private GameObject FindUsable(string key)
    {
        if (cache.ContainsKey(key))
        {
            //删除key对应的列表中所有的空引用
            cache[key].RemoveAll(p => p == null);
            //从列表中找出未激活的对象
            return cache[key].Find(p => !p.activeSelf);
        }
        return null;
    }
    /// <summary>加入缓存</summary>
    private void Add(string key, GameObject go)
    {
        //如果缓存中没有对应的key，先创建key及对应的列表
        if (!cache.ContainsKey(key))
            cache.Add(key, new List<GameObject>());
        //将对象放入缓存
        cache[key].Add(go);
    }
    /// <summary>协程做延时处理</summary>
    private IEnumerator DelayCollect(GameObject go, float delay)
    {
        //等待delay时间过去
        yield return new WaitForSeconds(delay);
        //再隐藏
        CollectObject(go);
    }
    /**********************************************************************************************/

    /**************************************** 自动 ************************************************/
    public override void Init()
    {
        base.Init();
    }
    /**********************************************************************************************/
}