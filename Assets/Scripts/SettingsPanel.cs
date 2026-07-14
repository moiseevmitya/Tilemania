using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Slider musicSlider, soundEffectsSlider;
    private int currentResolutionIndex;
    private int currentQualityIndex;
    private float currentMusicFloat, currentSoundEffectsFloat;
    private bool isFullscreen;

    Resolution[] resolutions;

    // Список поддерживаемых разрешений, которые будут отображаться в меню
    List<Vector2Int> commonResolutions = new List<Vector2Int>()
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1366, 768),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440),
        new Vector2Int(3840, 2160)
    };
    
    
    private void OnEnable()
    {
        CursorController.Show();
    }
    
    void Start()
    {
        currentMusicFloat = AudioSettings.instance.musicFloat;
        currentSoundEffectsFloat = AudioSettings.instance.soundEffectsFloat;
        
        currentQualityIndex = QualitySettings.GetQualityLevel();
        isFullscreen = Screen.fullScreen;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        List<Resolution> validResolutions = new List<Resolution>();
        resolutions = Screen.resolutions;

        // Нужен для удаления дубликатов разрешений с разной частотой обновления
        HashSet<string> seen = new HashSet<string>();

        Vector2Int currentResolution = new Vector2Int(Screen.width, Screen.height);

        // Добавляем наше текущее разрешение в список, если его там нет
        if (!commonResolutions.Contains(currentResolution))
        {
            commonResolutions.Add(currentResolution);
        }

        
        foreach (Resolution res in resolutions)
        {
            Vector2Int simpleRes = new Vector2Int(res.width, res.height);
            string key = res.width + "x" + res.height;

            if(!commonResolutions.Contains(simpleRes)) continue;

            if(seen.Contains(key)) continue;

            seen.Add(key);

            string option = key;
            options.Add(option);
            validResolutions.Add(res);

            // Запоминаем индекс текущего разрешения, чтобы сразу выбрать его в выпадающем списке
            if(res.width == Screen.width && res.height == Screen.height)
            {
                currentResolutionIndex = validResolutions.Count - 1;
            }
        }
        resolutions = validResolutions.ToArray();
        resolutionDropdown.AddOptions(options);
        LoadSettings();
        resolutionDropdown.RefreshShownValue();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool checkFullscreen)
    {
        Screen.fullScreen = checkFullscreen;
    }

    public void SliderSetMusicVolume(float musicVolume)
    {
        AudioSettings.instance.SetMusicVolume(musicVolume);
    }

    public void SliderSetSoundEffectsVolume(float soundEffectsVolume)
    {
        AudioSettings.instance.SetSoundEffectsVolume(soundEffectsVolume);
    }


    public void SaveSettings()
    {
        Resolution selectedResolution = resolutions[resolutionDropdown.value];
        PlayerPrefs.SetInt(PrefsKeys.ResolutionWidth, selectedResolution.width);
        PlayerPrefs.SetInt(PrefsKeys.ResolutionHeight, selectedResolution.height);
        
        PlayerPrefs.SetInt(PrefsKeys.Quality, qualityDropdown.value);
        PlayerPrefs.SetInt(PrefsKeys.Fullscreen, System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat(PrefsKeys.MusicVolume, musicSlider.value);
        PlayerPrefs.SetFloat(PrefsKeys.SoundEffectsVolume, soundEffectsSlider.value);
        PlayerPrefs.Save();

        // После сохранения обновляем локальные значения, чтобы при выходе не был откат к старым настройкам

        currentQualityIndex = qualityDropdown.value;
        currentResolutionIndex = resolutionDropdown.value;
        isFullscreen = Screen.fullScreen;
        currentMusicFloat = musicSlider.value;
        currentSoundEffectsFloat = soundEffectsSlider.value;
    }

    public void ExitSettings()
    {
        Screen.fullScreen = isFullscreen;

        if(resolutionDropdown.value != currentResolutionIndex)
        {
            SetResolution(currentResolutionIndex);
        }

        if(qualityDropdown.value != currentQualityIndex)
        {
            SetQuality(currentQualityIndex);
        }

        if(musicSlider.value != currentMusicFloat)
        {
            SliderSetMusicVolume(currentMusicFloat);
        }

        if(soundEffectsSlider.value != currentSoundEffectsFloat)
        {
            SliderSetSoundEffectsVolume(currentSoundEffectsFloat);
        }

        
        CursorController.Show();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    // Обновление UI элементов на текущие значения настроек
    private void LoadSettings()
    {
        qualityDropdown.value = currentQualityIndex;
        resolutionDropdown.value = currentResolutionIndex;
        fullscreenToggle.SetIsOnWithoutNotify(isFullscreen);
        musicSlider.value = currentMusicFloat;
        soundEffectsSlider.value = currentSoundEffectsFloat;
    }
}
