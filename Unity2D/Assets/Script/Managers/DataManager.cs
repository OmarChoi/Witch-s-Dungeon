using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public int width = 1920;
    public int height = 1080;
    public float musicVolume = 0.5f;
    public float effectVolume = 0.5f;
    public bool fullScreen = true;
}

public class DataManager
{
    Dictionary<string, Define.Status> enemyDictionary = new Dictionary<string, Define.Status>(); 
    Dictionary<string, Define.WeaponData[]> weaponDictionary = new Dictionary<string, Define.WeaponData[]>(Define.MaxWeaponLevel);
    Dictionary<string, string[]> weaponDescriptor = new Dictionary<string, string[]>();
    int[,] monsterAtMinuate = new int[Define.TotalTime, (int)Define.Monster.MonsterTypeCount];
    Define.Status playerStatus = new Define.Status(100.0f, 1.0f, 5.0f, 1.0f, 0);
    SettingData settingData = new SettingData();
    public int ScreenWidth { get { return settingData.width; } set { settingData.width = value; } }
    public int ScreenHeight { get { return settingData.height; } set { settingData.height = value; } }
    public float MusicVolume { get { return settingData.musicVolume; } set { settingData.musicVolume = value; } }
    public float EffectVolume { get { return settingData.effectVolume; } set { settingData.effectVolume = value; } }
    public bool IsFullScreen { get { return settingData.fullScreen; } set { settingData.fullScreen = value; } }

    public string savePath = "";

    public void Init()
    {

        savePath = Path.Combine(Application.dataPath + "/Data/", "settings.json");
        LoadSettings();
        LoadEnemyData();
        LoadMonstersAtTime();
        LoadWeaponDescriptor();
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
                    data[headers[j].Trim()] = float.Parse(values[j].Trim());
                }

                enemyDictionary.Add(monsterName, new Define.Status(
                    data["Hp"],
                    data["AttackSpeed"],
                    data["Speed"],
                    data["Damage"],
                    data["Exp"]
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

    private Define.WeaponData ParseWeaponData(string[] values, ref Define.WeaponData weaponData)
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

    private void LoadWeaponDescriptor()
    {
        TextAsset csvFile = LoadFile("WeaponDescriptor");
        string[] lines = csvFile.text.Split('\n');

        if (lines.Length > 0)
        {
            for (int i = 1; i < lines.Length; ++i)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] values = lines[i].Split(',');
                string weaponName = values[0];
                string weaponExplain = values[1];
                string weaponIncrease = values[2];
                string[] value = new string[] { weaponExplain, weaponIncrease };
                weaponDescriptor.Add(weaponName, value);
            }
        }
    }

    public void LoadSettings()
    {
        if (!File.Exists(savePath))
        {
            string newFile = JsonUtility.ToJson(settingData, true);
            File.WriteAllText(savePath, newFile);
        }
        string json = File.ReadAllText(savePath);
        settingData = JsonUtility.FromJson<SettingData>(json);
        SettingVolume(settingData.musicVolume, settingData.effectVolume);
        SettingScreenSize(settingData.width, settingData.height, settingData.fullScreen);
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

    public void GetPlayerStatus(out Define.Status status)
    {
        status = playerStatus;
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

    public int GetRequiredExpPerLevel(int level)
    {
        // Level에 따른 경험치 증가 수식
        // return (int)(MathF.Log10(level * level * level * 1000) + level) * 8;
        return level * level + level * 20;
    }

    public string GetWeaponDescriptor(string weaponName, bool IsFirstSelect)
    {
        if (!weaponDescriptor.ContainsKey(weaponName))
        {
            Debug.LogError($"{weaponName} Doesn't Exist in WeaponDescriptor ");
            return null;
        }
        if (IsFirstSelect)
            return weaponDescriptor[weaponName][0];
        else
            return weaponDescriptor[weaponName][1];
    }
    #endregion

    #region Save Data
    public void SettingScreenSize(int width,  int height, bool fullScreen)
    {
        settingData.width = width; 
        settingData.height = height;
        settingData.fullScreen = fullScreen;
        Screen.SetResolution(width, height, fullScreen);
    }

    public void SettingVolume(float music, float effect)
    {
        settingData.musicVolume = music;
        settingData.effectVolume = effect;
        Managers.Audio.SetAudioVolume("Music", settingData.musicVolume);
        Managers.Audio.SetAudioVolume("Effect", settingData.effectVolume);
    }

    public void SaveSettings()
    {
        Debug.Log("SaveSettings");
        string json = JsonUtility.ToJson(settingData, true);
        File.WriteAllText(savePath, json);
    }
    #endregion

    public void Clear()
    {
        SaveSettings();
    }
}
