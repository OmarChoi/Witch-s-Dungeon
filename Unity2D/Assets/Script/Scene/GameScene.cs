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

    double deltaTime = -1;
    double endTime = 5;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        playerCharacter = UnityEngine.Object.Instantiate(player);

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
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        #region Test Code
        string name = "Ghost";
        for (int j = 0; j < 10; ++j)
        {
            GameObject monster = Managers.Pool.GetObject(name);
            monsters.Add(monster);
            EnemyController controller = monster.GetComponent<EnemyController>();
            if (controller == null) 
            {
                Debug.LogError("Monster Controller Doesn't Exist");
                Application.Quit();
            }
            int xPos = UnityEngine.Random.Range(-5, 5);
            int yPos = UnityEngine.Random.Range(-5, 5);
            controller.SpawnMonster(playerCharacter, new Vector2(xPos, yPos), j);
        }
        #endregion
    }

    IEnumerator StartTimer()
    {
        while (deltaTime < endTime)
        {
            deltaTime += 1;
            sceneUI.UpdateTimer(deltaTime);
            yield return new WaitForSeconds(1);
        }
        yield break;
    }
}
