using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Text killCount;
    [SerializeField] Text surviveTime;

    bool isActive = false;
    public void Init()
    {
        this.gameObject.SetActive(true);
        GameScene gameScene = Managers.Scene.CurrentScene.GetComponent<GameScene>();
        if (gameScene == null) { return; }
        killCount.text = $"Kill Count : {gameScene.KiilCount}";
        surviveTime.text = $"Survival Time : {(int)gameScene.deltaTime / 60}m {(int)gameScene.deltaTime % 60}s";
        Invoke("SetActive", 1.0f);
    }

    private void SetActive()
    {
        isActive = true;
    }

    void Update()
    {
        if (isActive)
        {
            if (Input.anyKey)
            {
                Managers.Player.SetActive(false);
                Managers.UI.GetUIObject(name).SetActive(false);
                Managers.Scene.LoadScene("Lobby");
            }
        }
    }
}
