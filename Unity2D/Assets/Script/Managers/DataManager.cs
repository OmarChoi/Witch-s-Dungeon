using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DataManager
{
    Dictionary<string, Define.Status> enemyDictionary = new Dictionary<string, Define.Status>(); 
    Dictionary<string, Define.WeaponData[]> weaponDictionary = new Dictionary<string, Define.WeaponData[]>(Define.MaxWeaponLevel);
    int[,] monsterAtMinuate = new int[Define.TotalTime, (int)Define.Monster.MonsterTypeCount];

    public void Init()
    {
        LoadEnemyData();
        LoadMonstersAtTime();
        LoadWeaponDataPerLevel();
    }

    #region Load Data
    private TextAsset LoadFile(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"Data/{path}");
        if (csvFile == null)
        {
            Debug.LogError($"{path}.csv Doesn't Exist");
        }
        return csvFile;
    }

    private void LoadEnemyData()
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

                enemyDictionary.Add(monsterName, new Define.Status(
                    data["Hp"],
                    data["Speed"],
                    data["Damage"]
                ));
            }
        }
    }

    private void LoadMonstersAtTime()
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

    private void LoadWeaponDataPerLevel()
    {
        TextAsset[] csvFiles = Resources.LoadAll<TextAsset>("Data/Weapons");
        foreach (TextAsset csvFile in csvFiles)
        {
            
            string[] lines = csvFile.text.Split('\n');
            string weaponName = csvFile.name.Replace("Data", "").Trim();
            Define.WeaponData[] weaponData = new Define.WeaponData[Define.MaxWeaponLevel];
            if (lines.Length > 0)
            {
                for (int i = 1; i < lines.Length; ++i)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    string[] values = lines[i].Split(',');
                    int index = int.Parse(values[0].Trim());
                    ParseWeaponData(values, ref weaponData[index]);
                }
            }
            weaponDictionary.Add(weaponName, weaponData);
        }
    }

    private static Define.WeaponData ParseWeaponData(string[] values, ref Define.WeaponData weaponData)
    {
        FieldInfo[] fields = typeof(Define.WeaponData).GetFields();

        for (int i = 0; i < fields.Length; i++)
        {
            Type fieldType = fields[i].FieldType;
            if (fieldType == typeof(float))
            {
                fields[i].SetValueDirect(__makeref(weaponData), float.Parse(values[i + 1]));
            }
            else if (fieldType == typeof(int))
            {
                fields[i].SetValueDirect(__makeref(weaponData), int.Parse(values[i + 1]));
            }
        }
        return weaponData;
    }
    #endregion

    #region Get Data
    public void GetMonsterStatusByName(string monsterName, out Define.Status status)
    {
        if (enemyDictionary.ContainsKey(monsterName))
        {
            status = enemyDictionary[monsterName];
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


    public void GetWeaponData(string weaponName, int currentLevel, ref Define.WeaponData weaponData)
    {
        if(weaponDictionary.TryGetValue(weaponName, out Define.WeaponData[] weaponDataArray))
        {
            int level = Mathf.Clamp(Define.MaxWeaponLevel, 0, currentLevel);
            weaponData = weaponDataArray[level];
        }
        else
        {
            Debug.LogError($"Get Weapon Data Error : Can't Read {weaponName} in Dictionary");
        }
    }

    public int GetExpPerLevel(int level)
    {
        return 10;
    }

    #endregion
}
