using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Define;

public class GameScene : MonoBehaviour
{
    public GameObject playerCharacter;
    List<GameObject> monsters = new List<GameObject>();
    GameSceneUI sceneUI;
    double deltaTime = -1;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/GameSceneUI");
        GameObject ui = UnityEngine.Object.Instantiate(go);
        sceneUI = ui.GetComponent<GameSceneUI>();

        GameObject monsterFolder = new GameObject { name = "Mosnters" };
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Monster");

        GameObject weaponFolder= new GameObject { name = "Weapons" };
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Weapons");

        for (int i = 0; i < (int)Define.Weapon.WeaponTypeCount; ++i)
        {
            string name = Enum.GetName(typeof(Define.Weapon), i);
            Managers.Pool.CreatePool(name, 1, weaponFolder.transform);
        }

        for(int i = 0; i < (int)Define.Projectile.ProjectileTypeCount; ++i)
        {
            string name = Enum.GetName(typeof(Define.Projectile), i);
            Managers.Pool.CreatePool(name, 20, weaponFolder.transform);
        }

        for (int i = 0 ; i < (int)Define.Monster.MonsterTypeCount;++i)
        {
            string name = Enum.GetName(typeof(Define.Monster), i);
            Managers.Pool.CreatePool(name, 20, monsterFolder.transform);
        }

        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        playerCharacter = UnityEngine.Object.Instantiate(player);
        Managers.Player = playerCharacter;

        GameObject mapPrefab = Resources.Load<GameObject>("Prefabs/Map");
        GameObject map = UnityEngine.Object.Instantiate(mapPrefab);
        map.name = mapPrefab.name;

        StartCoroutine(StartTimer());
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
                randomPos = (randomPos * SpawnRange) + (Vector2)playerCharacter.transform.position;
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

    private void Clear()
    {
        foreach (var monster in monsters)
        {
            string monsterName = Utils.GetNameExceptClone(monster.name);
            Managers.Pool.ReleaseObject(monsterName, monster);
        }
        playerCharacter.SetActive(false);
        sceneUI.gameObject.SetActive(false);
        deltaTime = 0;
    }

    public void GameEnd()
    {
        Clear();
    }
}
