using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase
{
    GameObject playerCharacter;
    List<GameObject> monsters = new List<GameObject>();
    public GameSceneUI sceneUI;
    public int KiilCount = 0;
    public double deltaTime = 0;

    private void Awake()
    {
        Cursor.visible = false;
        Managers.Scene.SetScene("Game", this.gameObject);
        Init();
    }

    public void Init()
    {
        GameObject scene = Managers.UI.GetUIObject("GameSceneUI");
        scene.SetActive(true);
        sceneUI = scene.GetComponent<GameSceneUI>();

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
        WeaponUI weaponUI = Managers.UI.GetUIObject("GameSceneUI").GetComponent<WeaponUI>();
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
                int randomRange = UnityEngine.Random.Range(15, Define.SpawnRange);
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
        Managers.UI.ActivateUI("LevelUpUI");
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

    public void GameEnd(bool printGameOverUI = true)
    {
        if (printGameOverUI)
            Managers.UI.GetUIObject("GameOverUI").GetComponent<GameOverUI>().Init();
        Clear();
    }
}
