using System.Collections.Generic;
using UnityEngine;

public class PoolObject
{
    private readonly string m_objName;
    private readonly Stack<GameObject> m_objects = new Stack<GameObject>();
    private readonly Transform m_parent;

    public PoolObject(string objName, int initialSize, Transform parent = null)
    {
        m_objName = objName;
        m_parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    private GameObject CreateObject(bool isActiveByDefault = false)
    {
        GameObject newObject = Managers.Resource.Instantiate(m_objName, m_parent);
        newObject.SetActive(isActiveByDefault);
        newObject.name = m_objName;
        m_objects.Push(newObject);
        return newObject;
    }

    public GameObject Get()
    {
        if (m_objects.Count == 0)
        {
            CreateObject(true);
        }

        GameObject obj = m_objects.Pop();
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        m_objects.Push(obj);
    }

}
