using UnityEngine;
using UnityEngine.UI;

public class SoundControlUI : MonoBehaviour
{
    [SerializeField] Slider musicSound;
    [SerializeField] Slider effectSound;
    public void Save()
    {
        Managers.Audio.SetAudioVolume("Music", musicSound.value);
        Managers.Audio.SetAudioVolume("Effect", effectSound.value);
        Managers.UI.DeActivateUI();
    }
}
