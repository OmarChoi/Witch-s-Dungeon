using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : MonoBehaviour
{
    GameObject playerCharacter;
    List<GameObject> monsters = new List<GameObject>();
    GameSceneUI sceneUI;
    GameOverUI endingUI;
    WeaponUI weaponUI;
    LevelUpUI levelUpUI;
    OptionUI optionUI;
    SoundControlUI soundControlUI;

    double deltaTime = 0;

    private void Awake()
    {
        Managers.Scene = this;
        Init();
    }

    public void Init()
    {
        GameObject[] uiPrefabs = Resources.LoadAll<GameObject>("Prefabs/UI");
        foreach (GameObject prefab in uiPrefabs)
        {
            string uiName = prefab.name;
            GameObject uiObject = UnityEngine.Object.Instantiate(prefab);
            switch (uiName)
            {
                case "GameOverUI":
                    uiObject.SetActive(false);
                    endingUI = uiObject.GetComponent<GameOverUI>();
                    break;
                case "GameSceneUI":
                    sceneUI = uiObject.GetComponent<GameSceneUI>();
                    weaponUI = uiObject.GetComponent<WeaponUI>();
                    break;
                case "LevelUpUI":
                    uiObject.SetActive(false);
                    levelUpUI = uiObject.GetComponent<LevelUpUI>();
                    break;
                case "Option":
                    uiObject.SetActive(false);
                    optionUI = uiObject.GetComponent<OptionUI>();
                    break;
                case "SoundController":
                    uiObject.SetActive(false);
                    soundControlUI = uiObject.GetComponent<SoundControlUI>();
                    break;
            }
        }

        GameObject monsterFolder = new GameObject { name = "Mosnters" };
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Monster");

        GameObject weaponFolder= new GameObject { name = "Weapons" };
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Weapons");

        for (int i = 0; i < (int)Define.Weapon.WeaponTypeCount; ++i)
        {
            string name = Enum.GetName(typeof(Define.Weapon), i);
            Managers.Pool.CreatePool(name, 1);
        }

        for(int i = 0; i < (int)Define.Projectile.ProjectileTypeCount; ++i)
        {
            string name = Enum.GetName(typeof(Define.Projectile), i);
            Managers.Pool.CreatePool(name, 20);
        }

        for (int i = 0 ; i < (int)Define.Monster.MonsterTypeCount;++i)
        {
            string name = Enum.GetName(typeof(Define.Monster), i);
            Managers.Pool.CreatePool(name, 20);
        }

        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        playerCharacter = UnityEngine.Object.Instantiate(player);
        Managers.Player = playerCharacter;

        GameObject mapPrefab = Resources.Load<GameObject>("Prefabs/Map");
        GameObject map = UnityEngine.Object.Instantiate(mapPrefab);
        map.name = mapPrefab.name;

        FadeUI fadeUI = GameObject.Find("FadeUI").GetComponent<FadeUI>();
        fadeUI.FadeOut(2.0f);
        StartCoroutine(StartTimer());
    }

    public void ChangeWeaponLevel(int idx, int level)
    {
        int settingLevel = Mathf.Clamp(level, 0, Define.MaxWeaponLevel - 1);
        weaponUI.ChangeWeaponLevel(idx, settingLevel);
    }

    private void SpawnEnemy()
    {
        int minuate = (int)deltaTime / 60;
        if (minuate >= 15) return;
        for (int i = 0; i < (int)Define.Monster.MonsterTypeCount; ++i)
        {
            string name = Enum.GetName (typeof(Define.Monster), i);
            int nMonsters = Managers.Data.GetMonsterDataByTime(minuate, i);
            for (int j = 0; j < nMonsters; ++j)
            {
                Vector2 randomPos = UnityEngine.Random.insideUnitCircle;
                int randomRange = UnityEngine.Random.Range(10, Define.SpawnRange);
                randomPos = (randomPos * randomRange) + (Vector2)playerCharacter.transform.position;
                GameObject monster = Managers.Pool.GetObject(name, randomPos);
                monsters.Add(monster);
                EnemyController controller = monster.GetComponent<EnemyController>();
                if (controller == null)
                {
                    Debug.LogError($"{name} doesn't have controller");
                }
                controller.SpawnMonster(playerCharacter, randomPos);
            }
        }
    }

    IEnumerator StartTimer()
    {
        while (deltaTime < Define.TotalTime * 60)
        {
            deltaTime += 1;
            sceneUI.UpdateTimer(deltaTime);
            if(deltaTime % Define.SpawnCycle == 0)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(1);
        }
        GameEnd();
        yield break;
    }

    public void SpawnLevelUpUI()
    {
        Time.timeScale = 0;
        levelUpUI.SpawnLevelUpUI();
    }

    private void Clear()
    {
        StopAllCoroutines();
        foreach (var monster in monsters)
        {
            string monsterName = Utils.GetNameExceptClone(monster.name);
            Managers.Pool.ReleaseObject(monsterName, monster);
        }
        sceneUI.gameObject.SetActive(false);
        deltaTime = 0;
    }

    public void GameEnd()
    {
        endingUI.Init();
        Clear();
    }
}
