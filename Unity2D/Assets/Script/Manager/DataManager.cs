using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public Dictionary<string, Define.Status> enemyData = new Dictionary<string, Define.Status>();

    public void Init()
    {
        LoadEnemyData();
    }

    public void LoadEnemyData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Data/EnemyInfo");
        if (csvFile == null)
        {
            Debug.LogError("EnemyInfo.csv Doesn't Exist");
        }
        string[] lines = csvFile.text.Split('\n');

        if (lines.Length > 0)
        {
            string[] headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] values = lines[i].Split(',');
                var data = new Dictionary<string, float>();
                string monsterName = values[0].Trim();
                for (int j = 1; j < headers.Length; j++)
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
}
