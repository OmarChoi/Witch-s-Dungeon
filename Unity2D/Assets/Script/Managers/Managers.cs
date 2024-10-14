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

    static GameObject playerCharacter = null;
    static GameScene gameScene = null;
    

    public static ResourceManager Resource { get { return Manager.resourceManager; } }
    public static PoolManager Pool { get { return Manager.poolManager; } }
    public static DataManager Data { get { return Manager.dataManager; } }
    public static SoundManager Audio { get { return Manager.soundManager; } }

    public static GameObject Player { get { return playerCharacter; } 
        set
        { 
            if (playerCharacter == null)
            {
                playerCharacter = value;
            }
        }
    }

    public static GameScene Scene { get { return gameScene; } set { gameScene = value; } }

    static void Init() 
    {
        if (s_Manager == null) 
        {
            Debug.Log("Init Manager");
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
            Data.Init();
            Audio.Init();
        }
    }

    public static void Clear() 
    {
        s_Manager = null;
    }
}
