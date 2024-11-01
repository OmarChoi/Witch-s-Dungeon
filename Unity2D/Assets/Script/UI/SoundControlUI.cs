using UnityEngine;
using UnityEngine.UI;

public class SoundControlUI : MonoBehaviour
{
    [SerializeField] Slider musicSound;
    [SerializeField] Slider effectSound;

    public void OnEnable()
    {
        musicSound.value = Managers.Audio.GetVolume("Music");
        effectSound.value = Managers.Audio.GetVolume("Effect");
    }

    public void Save()
    {
        Managers.Data.SettingVolume(musicSound.value, effectSound.value);
        Managers.UI.DeActivateUI();
    }
}
