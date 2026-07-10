using NUnit.Framework;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings instance { get; private set; }
    public AudioSource musicAudio;
    public AudioSource soundEffectsAudio;
    public float musicFloat { get; private set; }
    public float soundEffectsFloat { get; private set; }

    private void Awake()
    {
        // Оставляем только один экземпляр AudioSettings, чтобы музыка и настройки не сбрасывались при смене сцен
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }    
        
        instance = this;
        DontDestroyOnLoad(gameObject);

        musicFloat = PlayerPrefs.GetFloat(PrefsKeys.MusicVolume, 0.5f);
        soundEffectsFloat = PlayerPrefs.GetFloat(PrefsKeys.SoundEffectsVolume, 0.5f);

        SetMusicVolume(musicFloat);
        SetSoundEffectsVolume(soundEffectsFloat);
    }

    public void SetMusicVolume(float musicVolume)
    {
        musicFloat = musicVolume;
        musicAudio.volume = musicFloat;
        
    }

    public void SetSoundEffectsVolume(float soundEffectsVolume)
    {
        soundEffectsFloat = soundEffectsVolume;
        soundEffectsAudio.volume = soundEffectsFloat;
    }

    public void SetAndPlayMusic(AudioClip audioClip)
    {
        if(musicAudio.clip == audioClip) return;
        musicAudio.clip = audioClip;
        musicAudio.Play();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if(soundEffectsAudio.isPlaying && soundEffectsAudio.clip == audioClip) return;
        soundEffectsAudio.clip = audioClip;
        soundEffectsAudio.Play();
    }
}
