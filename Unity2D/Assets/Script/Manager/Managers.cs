using UnityEditor.EditorTools;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Manager;
    static Managers Manager { get { Init(); return s_Manager; } }

    ResourceManager resourceManager = new ResourceManager();
    PoolManager poolManager = new PoolManager();
    DataManager dataManager = new DataManager();
    UIManager uiManager = new UIManager();
    public static ResourceManager Resource { get { return Manager.resourceManager; } }
    public static PoolManager Pool { get { return Manager.poolManager; } }
    public static DataManager Data { get { return Manager.dataManager; } }
    public static UIManager UI { get { return Manager.uiManager; } }

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
            Data.Init();
        }
    }

    public static void Clear() 
    {
        s_Manager = null;
    }
}
