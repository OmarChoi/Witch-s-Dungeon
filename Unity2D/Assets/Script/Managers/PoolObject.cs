using System.Collections.Generic;
using UnityEngine;

public class PoolObject
{
    private string m_objName;
    private Stack<GameObject> m_objects = new Stack<GameObject>();
    private Transform m_parent;

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
        m_objects.Push(newObject);
        return newObject;
    }

    public GameObject Get(Vector3 position = default(Vector3))
    {
        if (m_objects.Count == 0)
        {
            CreateObject(true);
        }

        GameObject obj = m_objects.Pop();
        obj.transform.position = position;
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
