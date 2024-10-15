using UnityEngine;

public class LobbyScene : SceneBase
{
    public void Awake()
    {
        Managers.Scene.SetScene("Lobby", this.gameObject);
#if !UNITY_ANDROID
        Screen.SetResolution(1920, 1080, false);
#endif
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.DeActivateUI();
        }
    }
}
