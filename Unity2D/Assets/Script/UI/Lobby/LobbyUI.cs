using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    FadeUI fadeOut;
    [SerializeField] GameObject optionUI;

    private void Awake()
    {
        fadeOut = GetComponentInParent<FadeUI>();
    }

    public void ButtonClicked(string name)
    {
        switch (name)
        {
            case "GameStart":
                float fadeDuration = 1.0f;
                fadeOut.FadeIn(fadeDuration);
                Invoke("LoadGameScene", fadeDuration);
                break;
            case "Option":
                Managers.UI.ActivateUI("OptionUI");
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }

    private void LoadGameScene()
    {
        Managers.Scene.LoadScene("Game");
    }
}
