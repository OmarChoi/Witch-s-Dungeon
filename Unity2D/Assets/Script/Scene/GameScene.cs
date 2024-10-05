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
    GameObject[] weapons = new GameObject[(int)Define.Weapon.WeaponTypeCount];
    GameSceneUI sceneUI;
    double deltaTime = -1;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        playerCharacter = UnityEngine.Object.Instantiate(player);
        Managers.Player = playerCharacter;

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
            GameObject weaponPrefab = Managers.Resource.GetResource<GameObject>(name);
            GameObject weapon = UnityEngine.Object.Instantiate(weaponPrefab);
            weapon.transform.SetParent(weaponFolder.transform);
            weapon.SetActive(false);
            weapons[i] = weapon;
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

        StartCoroutine(StartTimer());
        SpawnProjectile("Shuriken");
    }

    private void ActiveWeapon(int index)
    {
        if(index > weapons.Length)
        {
            Debug.LogError("Wrong Acess at [GameScene weapon]");
            return;
        }
        weapons[index].SetActive(true);
        weapons[index].GetComponent<WeaponBase>().Init();
    }

    private void SpawnProjectile(string projectileName)
    {
        Vector2 startPos = Managers.Player.transform.position;
        GameObject projectile = Managers.Pool.GetObject(projectileName, startPos);
        projectile.GetComponent<WeaponBase>().Init();
    }

    private void SpawnEnemy()
    {
        int minuate = (int)deltaTime / 60;
        
        for (int i = 0; i < (int)Define.Monster.MonsterTypeCount; ++i)
        {
            string name = Enum.GetName (typeof(Define.Monster), i);
            int nMonsters = Managers.Data.GetMonsterDataByTime(minuate, i);
            for (int j = 0; j < nMonsters; ++j)
            {
                Vector3 randomPos = UnityEngine.Random.insideUnitCircle * SpawnRange;
                randomPos = UnityEngine.Random.insideUnitCircle * SpawnRange + (Vector2)playerCharacter.transform.position + new Vector2(5.0f, 5.0f);
                GameObject monster = Managers.Pool.GetObject(name, randomPos);
                monsters.Add(monster);
                EnemyController controller = monster.GetComponent<EnemyController>();
                if (controller == null)
                {
                    Debug.LogError($"{name} doesn't have controller");
                }
                controller.SpawnMonster(playerCharacter, randomPos, monsters.Count);
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
        yield break;
    }
}
