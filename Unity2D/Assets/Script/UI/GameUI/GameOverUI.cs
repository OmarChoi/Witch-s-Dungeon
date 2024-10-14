using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    bool isActive = false;
    public void Init()
    {
        this.gameObject.SetActive(true);
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
                SceneManager.LoadScene("Lobby");
            }
        }
    }
}
