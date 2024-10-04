using System;
using System.Collections;
using System.Collections.Generic;
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
        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        playerCharacter = UnityEngine.Object.Instantiate(player);
        Managers.Player = playerCharacter;

        GameObject go = Resources.Load<GameObject>("Prefabs/GameSceneUI");
        GameObject ui = UnityEngine.Object.Instantiate(go);
        sceneUI = ui.GetComponent<GameSceneUI>();

        GameObject monsterFolder = new GameObject { name = "Mosnters" };
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Monster");

        for(int i = 0 ; i < (int)Define.Monster.MonsterTypeCount;++i)
        {
            string name = Enum.GetName(typeof(Define.Monster), i);
            Managers.Pool.CreatePool(name, 20, monsterFolder.transform);
        }

        StartCoroutine(StartTimer());
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
                GameObject monster = Managers.Pool.GetObject(name);
                monsters.Add(monster);
                EnemyController controller = monster.GetComponent<EnemyController>();
                if (controller == null)
                {
                    Debug.LogError($"{name} doesn't have controller");
                }
                Vector2 randomPos = UnityEngine.Random.insideUnitCircle * SpawnRange;
                randomPos = UnityEngine.Random.insideUnitCircle * SpawnRange + (Vector2)playerCharacter.transform.position + new Vector2(5.0f, 5.0f);
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
