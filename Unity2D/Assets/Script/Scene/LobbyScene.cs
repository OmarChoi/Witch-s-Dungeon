using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    public void Awake()
    {
        Managers.Audio.SetAudioVolume("Music", 0.5f);
        Managers.Audio.SetAudioVolume("Effect", 0.5f);
        Screen.SetResolution(1920, 1080, false);
    }
}
