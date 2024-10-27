using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void OnEnable()
    {
        Managers.Audio.PauseAudio();
    }

    public void ButtonClicked(string name)
    {
        switch (name)
        {
            case "Resume":
                Managers.Audio.ResumeAudio();
                Managers.UI.DeActivateUI();
                break;
            case "Quit":
                Managers.UI.DeActivateUI();
                Managers.Scene.LoadScene("Lobby");
                break;
        }
    }
}
                                                        