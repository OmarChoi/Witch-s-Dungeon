using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{

    private Dictionary<string, Object> m_resourceCache;

    public void Init()
    {
        m_resourceCache = new Dictionary<string, Object>();
    }

    public void LoadResource<T>(string path) where T : Object
    {
        string fileName = Path.GetFileName(path);

        T resource = Resources.Load<T>(path);
        if (resource != null)
        {
            m_resourceCache[fileName] = resource;
        }
        else
        {
            Debug.LogWarning($"Resource at path {path} not found.");
        }
    }

    public void LoadResourcesInFolder<T>(string folderPath) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(folderPath);
        foreach (T resource in resources)
        {
            string resourcePath = folderPath + "/" + resource.name;
            string fileName = Path.GetFileName(resourcePath);

            if (!m_resourceCache.ContainsKey(fileName))
            {
                m_resourceCache[fileName] = resource;
            }
        }
    }

    public void RemoveResource(string path)
    {
        string fileName = Path.GetFileName(path);

        if (m_resourceCache.ContainsKey(fileName))
        {
            m_resourceCache.Remove(fileName);
        }
    }

    public T GetResource<T>(string path) where T : Object
    {
        string fileName = Path.GetFileName(path);

        if (m_resourceCache.ContainsKey(fileName))
        {
            return m_resourceCache[fileName] as T;
        }
        Debug.LogWarning($"Resource with name {fileName} not found in cache.");
        return null;
    }

    public GameObject Instantiate(string fileName, Transform parent = null)
    {
        if (!m_resourceCache.ContainsKey(fileName))
        {
            string path = $"Prefabs/{fileName}";
            LoadResource<GameObject>(path);
        }
        GameObject prefab = m_resourceCache[fileName] as GameObject;
        GameObject gameObject = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        gameObject.name = prefab.name;
        gameObject.transform.parent = parent;
        return gameObject;
    }

    public void Clear()
    {
        m_resourceCache.Clear();
    }
}
