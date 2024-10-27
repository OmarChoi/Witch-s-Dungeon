using UnityEngine;
using UnityEngine.Audio;

public class Managers : MonoBehaviour
{
    static Managers s_Manager;
    static Managers Manager { get { Init(); return s_Manager; } }

    ResourceManager resourceManager = new ResourceManager();
    PoolManager poolManager = new PoolManager();
    DataManager dataManager = new DataManager();
    SoundManager soundManager= new SoundManager();
    UIManger uiManager= new UIManger();
    SceneManager sceneManager= new SceneManager();

    static GameObject playerCharacter = null;

    public static ResourceManager Resource { get { return Manager.resourceManager; } }
    public static PoolManager Pool { get { return Manager.poolManager; } }
    public static DataManager Data { get { return Manager.dataManager; } }
    public static SoundManager Audio { get { return Manager.soundManager; } }
    public static UIManger UI { get { return Manager.uiManager; } }
    public static SceneManager Scene { get { return Manager.sceneManager; } }

    public static GameObject Player { get { return playerCharacter; } 
        set
        { 
            if (playerCharacter == null)
            {
                playerCharacter = value;
            }
        }
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
                Object.DontDestroyOnLoad(obj);
            }
            s_Manager = obj.GetComponent<Managers>();
            Pool.Init();
            Resource.Init();
            Audio.Init();
            UI.Init();
            Data.Init();
            Scene.Init();
        }
    }
    private void OnApplicationQuit()
    {
        Clear();
    }

    public static void Clear() 
    {
        Data.Clear();
        s_Manager = null;
    }
}
