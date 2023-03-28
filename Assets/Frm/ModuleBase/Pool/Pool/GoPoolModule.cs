using System;
using System.Collections.Generic;
using UnityEngine;

public class GoPoolModule : IGoPoolModule
{
    private const string _PoolName = "_GOPool";
    private Dictionary<string, GoPoolData> pools;
    public GoPoolModule() => pools = new Dictionary<string, GoPoolData>();
    public GameObject Acquire(GameObject prefab)
    {
        string poolName = prefab.name + _PoolName;
        if (!pools.TryGetValue(poolName, out GoPoolData poolData))
        {
            Debug.LogError("没有合适的对象池" + poolName);
            return null;
        }
        GameObject go = null;
        while (poolData.validPool.Count > 0 && go == null)
            go = poolData.validPool.Dequeue();
        if(go!=null)
        {
            go.SetActive(true);
            return go;
        }
        if (poolData.pool.Count >= poolData.pool.Capacity)
        {
            Debug.LogError("超过池中最大数");
            return null;
        }

        go = GameObject.Instantiate(prefab);
        go.AddComponent<GoPoolEntity>().Init(OnGoDestroy);
        go.name = poolName;
        poolData.pool.Add(go);
        go.SetActive(true);

        return go;
    }

    public void Clear() => pools.Clear();

    /// <summary>
    /// 将游戏物体载入对象池
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="capacity"></param>
    public void Load(GameObject prefab, int capacity)
    {
        if(capacity<=0)
        {
            Debug.LogError("容量错误");
            return;
        }
        if(prefab==null)
        {
            Debug.LogError("载入预制体为空");
            return;
        }
        string poolName = prefab.name + _PoolName;
        if (poolName.Contains(poolName))
        {
            Debug.LogError("该预制体已注册");
            return;
        }
        pools.Add(poolName, new GoPoolData(capacity));
    }
    /// <summary>
    /// 将游戏物体回收;只有该物体创建出来了被回收采访到池中
    /// </summary>
    /// <param name="go"></param>
    public void Recycle(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("回收的实体为空");
            return;
        }
        if (!pools.TryGetValue(go.name,out  GoPoolData poolData))
        {
            Debug.LogError("没有合适的对象池"+go.name);
            return;
        }
        if (!poolData.pool.Contains(go))
        {
            Debug.LogError("该池子未创建实体");
            return;
        }
        go.SetActive(false);
        poolData.validPool.Enqueue(go);
    }

    public void Shrink()
    {
        foreach (GoPoolData goPoolData in pools.Values)
        {
            GameObject go;
            while ((go = goPoolData.validPool.Dequeue()) != null)
                GameObject.Destroy(go);
        }
    }
    /// <summary>
    /// 卸载该对象的整个池子
    /// </summary>
    /// <param name="prefab"></param>
    public void Unload(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("卸载不存在的预制体");
            return;
        }
        string poolname = prefab.name + _PoolName;
        if (!pools.TryGetValue(poolname,out GoPoolData poolData))
        {
            Debug.LogWarning("不存在该池子"+poolname);
            return;
        }
        for (int i = 0; i < poolData.pool.Count; i++)
            GameObject.Destroy(poolData.pool[i]);

        pools.Remove(poolname);
    }

    private void OnGoDestroy(GameObject go)
    {
        if (!pools.TryGetValue(go.name, out GoPoolData poolData))
            return;

        if (!poolData.pool.Contains(go))
            return;

        poolData.pool.Remove(go);
    }
}
