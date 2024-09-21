using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    GameObject playerCharacter;
    GameSceneUI sceneUI;
    List<ControllerBase> monsetrs = new List<ControllerBase>();

    double deltaTime = -1;
    double endTime = 5;

    void Start()
    {
        Init();
        SpawnEnemy();
    }

    void Update()
    {
        for(int i = 0; i < monsetrs.Count; i++)
        {
            EnemyController controller = monsetrs[i] as EnemyController;
            controller.UpdateTargetDirection(playerCharacter.transform.position);
        }
    }

    public void Init()
    {
        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        playerCharacter = UnityEngine.Object.Instantiate(player);

        GameObject go = Resources.Load<GameObject>("Prefabs/GameSceneUI");
        GameObject ui = UnityEngine.Object.Instantiate(go);
        sceneUI = ui.GetComponent<GameSceneUI>();

        #region TempCode
        Managers.Resource.LoadResourcesInFolder<GameObject>("Prefabs/Monster");
        string name = "Ghost";
        Managers.Pool.CreatePool(name, 20);
        #endregion

        StartCoroutine(StartTimer());
    }

    private void SpawnEnemy()
    {
        string name = "Ghost";
        for (int j = 0; j < 10; ++j)
        {
            GameObject monster = Managers.Pool.GetObject(name);
            EnemyController controller = monster.GetComponent<EnemyController>();
            if (controller == null) 
            {
                Debug.LogError("Monster Controller Doesn't Exist");
            }
            monsetrs.Add(controller);
            int xPos = UnityEngine.Random.Range(-5, 5);
            int yPos = UnityEngine.Random.Range(-5, 5);
            controller.SpawnMonster(new Vector2(xPos, yPos), j);
            controller.StartMove(playerCharacter.transform.position);
        }
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
