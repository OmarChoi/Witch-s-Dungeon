using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private Dictionary<string, PoolObject> m_pools = new Dictionary<string, PoolObject>();
    Transform m_poolRoot;

    public void Init()
    {
        if (m_poolRoot == null)
        {
            m_poolRoot = new GameObject { name = "Pool_Root" }.transform;
            Object.DontDestroyOnLoad(m_poolRoot);
        }
    }

    public void CreatePool(string key, int initialSize = 10, Transform parent = null)
    {
        if (!m_pools.ContainsKey(key))
        {
            GameObject gameObject = new GameObject { name = key };
            gameObject.transform.parent = m_poolRoot;
            PoolObject pool = new PoolObject(key, initialSize, gameObject.transform);
            m_pools[key] = pool;
        }
    }

    public GameObject GetObject(string key)
    {
        if (m_pools.ContainsKey(key))
        {
            return m_pools[key].Get();
        }
        Debug.LogError($"{key} Doesn't Exist");
        return null;
    }

    public void ReleaseObject(string key, GameObject obj)
    {
        if (m_pools.ContainsKey(key))
        {
            m_pools[key].Release(obj);
        }
    }
}