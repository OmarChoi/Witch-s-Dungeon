using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    static AudioMixer audioMixer = null;
    public void Init()
    {
        audioMixer = Resources.Load<AudioMixer>("Sound/SoundMixer");
    }

    public void SetAudioVolume (string type, float volume)
    {
        audioMixer.SetFloat(type, Mathf.Log10(volume) * 20);
    }
}