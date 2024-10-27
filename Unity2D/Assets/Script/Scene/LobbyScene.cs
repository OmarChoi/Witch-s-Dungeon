using UnityEngine;

public class LobbyScene : SceneBase
{
    public void Awake()
    {
        Cursor.visible = true;
        Managers.Scene.SetScene("Lobby", this.gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.DeActivateUI();
        }
    }
}
