using System.Collections.Generic;
using UnityEngine;

public class UIManger
{
    Dictionary<string, GameObject> uiObjectDictionary = new Dictionary<string, GameObject>();
    Stack<GameObject> ActiveUI = new Stack<GameObject>();
    int sortLayerIndex = 1;
    bool CanUseEscapeKey = true;

    public void Init()
    {
        GameObject uiFolder = new GameObject("UI");
        Object.DontDestroyOnLoad(uiFolder);
        GameObject[] uiPrefabs = Resources.LoadAll<GameObject>("Prefabs/UI");
        foreach (GameObject prefab in uiPrefabs)
        {
            string uiName = prefab.name;
            GameObject uiObject = UnityEngine.Object.Instantiate(prefab, uiFolder.transform);
            uiObject.name = uiName;
            uiObject.gameObject.SetActive(false);
            uiObjectDictionary.Add(uiName, uiObject);
        }
    }

    public void ActivateUI(string uiName)
    {
        Time.timeScale = 0.0f;
        if (uiObjectDictionary.ContainsKey(uiName))
        {
            uiObjectDictionary[uiName].SetActive(true);
            uiObjectDictionary[uiName].layer = sortLayerIndex;
            sortLayerIndex += 1;
            ActiveUI.Push(uiObjectDictionary[uiName]);
        }
    }

    public void DeActivateUI()
    {
        if (CanUseEscapeKey == false)
            return;
        if (ActiveUI.Count == 0)
        {
            switch (Managers.Scene.CurrentSceneName)
            {
                case "Lobby":
                    ActivateUI("ExitGameUI");
                    break;
                case "Game":
                    ActivateUI("PauseUI");
                    break;
            }
        }
        else
        {
            GameObject ui = ActiveUI.Pop();
            sortLayerIndex -= 1;
            ui.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void SetEscapeEnable(bool enable)
    {
        CanUseEscapeKey = enable;
    }

    public GameObject GetUIObject(string name)
    {
        if (uiObjectDictionary.ContainsKey(name))
            return uiObjectDictionary[name];
        return null;
    }
}
