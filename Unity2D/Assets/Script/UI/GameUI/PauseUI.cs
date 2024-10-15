using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void ButtonClicked(string name)
    {
        switch (name)
        {
            case "Resume":
                Managers.UI.DeActivateUI();
                break;
            case "Quit":
                Managers.UI.DeActivateUI();
                Managers.Scene.LoadScene("Lobby");
                break;
        }
    }
}
                                                        