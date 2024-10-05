using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    Dictionary<string, Define.Status> enemyData = new Dictionary<string, Define.Status>();
    int[,] monsterAtMinuate = new int[Define.TotalTime, (int)Define.Monster.MonsterTypeCount];

    public void Init()
    {
        LoadEnemyData();
        LoadMonstersAtTime();
    }

    private TextAsset LoadFile(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"Data/{path}");
        if (csvFile == null)
        {
            Debug.LogError($"{path}.csv Doesn't Exist");
        }
        return csvFile;
    }

    public void GetMonsterStatusByName(string monsterName, out Define.Status status)
    {
        if (enemyData.ContainsKey(monsterName))
        {
            status = enemyData[monsterName];
        }
        else
        {
            Debug.LogError($"{monsterName} doesn't exist");
            status = null;
        }
    }

    public int GetMonsterDataByTime(int time, int monsterType)
    {
        if (time < 0 || time >= Define.TotalTime)
        {
            Debug.LogError($"Get Monster Data Error : Index Error ");
        }
        return monsterAtMinuate[time, monsterType];
    }

    public void LoadEnemyData()
    {
        TextAsset csvFile = LoadFile("EnemyInfo");
        string[] lines = csvFile.text.Split('\n');

        if (lines.Length > 0)
        {
            string[] headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; ++i)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] values = lines[i].Split(',');
                var data = new Dictionary<string, float>();
                string monsterName = values[0].Trim();
                for (int j = 1; j < headers.Length; ++j)
                {
                    data[headers[j].Trim()] = int.Parse(values[j].Trim());
                }

                enemyData.Add(monsterName, new Define.Status(
                    data["Hp"],
                    data["Speed"],
                    data["Damage"]
                ));
            }
        }
    }

    public void LoadMonstersAtTime()
    {
        TextAsset csvFile = LoadFile("MonstersAtTime");
        string[] lines = csvFile.text.Split('\n');
        if (lines.Length > 0)
        {
            string[] headers = lines[0].Split(',');
            for (int i = 1; i < lines.Length; ++i)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] values = lines[i].Split(',');
                int time = int.Parse(values[0].Trim());
                for (int j = 1; j < headers.Length; ++j)
                {
                    monsterAtMinuate[time, j - 1] = int.Parse(values[j].Trim());
                }
            }
        }
    }

    public void LoadWeaponData()
    {

    }
}
