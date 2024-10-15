using UnityEngine;

public class Utils
{
    public static string GetNameExceptClone(string name)
    {
        return name.Replace("(Clone)", "").Trim();
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindObjectInChild(GameObject go, string name)
    {
        GameObject targetObject = null;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (targetObject != null) break;
            Transform transform = go.transform.GetChild(i);
            if(transform.name == name)
            {
                targetObject = transform.gameObject;
                return targetObject;
            }
            else
            {
                targetObject = FindObjectInChild(transform.gameObject, name);
            }
        }
        return targetObject;
    }
}
