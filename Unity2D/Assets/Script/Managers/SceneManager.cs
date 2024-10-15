using System;
using System.Xml.Linq;
using UnityEngine;

public class SceneManager
{
    enum Scene { Lobby = 0, Game = 1 }
    GameObject[] scenes = new GameObject[2];
    int currentSceneIndex = 0;

    public GameObject CurrentScene 
    { 
        get 
        {
            if (scenes[currentSceneIndex] == null)
                Debug.Log("SceneManager : Scene Doesn't Exist");
            return scenes[currentSceneIndex]; 
        } 
    }

    public string CurrentSceneName 
    { 
        get 
        {
            if (currentSceneIndex == (int)Scene.Lobby)
                return "Lobby";
            else 
                return "Game";
        }
        
    }

    public GameScene GameScene
    {
        get
        {
            if (currentSceneIndex == (int)Scene.Game)
                return scenes[currentSceneIndex].GetComponent<GameScene>();
            else
                return null;
        }
    }

    public LobbyScene LobbyScene
    {
        get
        {
            if (currentSceneIndex == (int)Scene.Lobby)
                return scenes[currentSceneIndex].GetComponent<LobbyScene>();
            else
                return null;
        }
    }

    public void Init()
    {
        CreateGameSceneObject();
    }

    public void SetScene(string name, GameObject scene)
    {
        if (name == "Lobby")
            scenes[(int)Scene.Lobby] = scene;
        else if (name == "Game")
            scenes[(int)Scene.Game] = scene;
    }

    public void LoadScene(string SceneName)
    {
        if (SceneName == "Lobby")
        {
            currentSceneIndex = (int)Scene.Lobby;
            scenes[(int)Scene.Game].GetComponent<GameScene>().GameEnd(false);
        }
        else if (SceneName == "Game")
            currentSceneIndex = (int)Scene.Game;
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    public void CreateGameSceneObject()
    {
        GameObject monsterFolder = new GameObject { name = "Mosnters" };
        GameObject weaponFolder = new GameObject { name = "Weapons" };
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Monster");
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Weapons");

        for (int i = 0; i < (int)Define.Weapon.WeaponTypeCount; ++i)
        {
            string name = Enum.GetName(typeof(Define.Weapon), i);
            Managers.Pool.CreatePool(name, 1, weaponFolder.transform);
        }

        for (int i = 0; i < (int)Define.Projectile.ProjectileTypeCount; ++i)
        {
            string name = Enum.GetName(typeof(Define.Projectile), i);
            Managers.Pool.CreatePool(name, 20, weaponFolder.transform);
        }

        for (int i = 0; i < (int)Define.Monster.MonsterTypeCount; ++i)
        {
            string name = Enum.GetName(typeof(Define.Monster), i);
            Managers.Pool.CreatePool(name, 20, monsterFolder.transform);
        }
    }
}
