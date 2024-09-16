using System.Collections;
using System.IO;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.UI.Image;

public class GameScene : MonoBehaviour
{
    GameObject playerCharacter;
    GameSceneUI sceneUI;

    double deltaTime = -1;
    double endTime = 5;

    void Start()
    {
        Init();
    }

    void Update()
    {
        SpawnEnemy();
    }

    public void Init()
    {
        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        playerCharacter = Object.Instantiate(player);

        GameObject go = Resources.Load<GameObject>("Prefabs/GameSceneUI");
        GameObject ui = Object.Instantiate(go);
        sceneUI = ui.GetComponent<GameSceneUI>();

        StartCoroutine(StartTimer());
    }

    private void SpawnEnemy()
    {

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
