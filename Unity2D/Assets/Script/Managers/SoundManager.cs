using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    static AudioMixer audioMixer = null;
    public AudioMixer Mixer { get { return audioMixer; } }

    public float[] AudioVolume = new float[2];

    public void Init()
    {
        audioMixer = Resources.Load<AudioMixer>("Sound/SoundMixer");
        Managers.Resource.LoadResourcesInFolder<AudioClip>("Sound/AudioClips/Effect");
    }

    private int GetVolumeTypeIndex(string type)
    {
        int volumeType = (type == "Music") ? 0 : 1;
        return volumeType;
    }

    public void SetAudioVolume (string type, float volume)
    {
        AudioVolume[GetVolumeTypeIndex(type)] = volume;
        audioMixer.SetFloat(type, Mathf.Log10(volume) * 20);
    }

    public float GetVolume(string type)
    {
        return AudioVolume[GetVolumeTypeIndex(type)];
    }


}