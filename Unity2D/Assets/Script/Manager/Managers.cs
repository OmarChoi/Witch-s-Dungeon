using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Manager;
    static Managers Manager { get { Init(); return s_Manager; } }

    private void Start()
    {
        Init();
    }

    static void Init() 
    {
        if (s_Manager == null) 
        {
            GameObject obj = GameObject.Find("ManagerSet");
            if (obj == null) 
            {
                obj = new GameObject { name = "ManagerSet" };
                obj.AddComponent<Managers>();
            }
            s_Manager = obj.GetComponent<Managers>();
        }
    }

    public static void Clear() 
    {
        s_Manager = null;
    }
}
