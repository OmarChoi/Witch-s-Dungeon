using UnityEditor.EditorTools;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Manager;
    static Managers Manager { get { Init(); return s_Manager; } }

    ResourceManager m_resourceManager = new ResourceManager();
    PoolManager m_poolManager = new PoolManager();
    DataManager m_dataManager = new DataManager();
    UIManager m_uiManager = new UIManager();
    public static ResourceManager Resource { get { return Manager.m_resourceManager; } }
    public static PoolManager Pool { get { return Manager.m_poolManager; } }
    public static DataManager Data { get { return Manager.m_dataManager; } }
    public static UIManager UI { get { return Manager.m_uiManager; } }

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
            Resource.Init();
        }
    }

    public static void Clear() 
    {
        s_Manager = null;
    }
}
