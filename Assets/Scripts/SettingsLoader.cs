using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    Resolution[] resolutions;

    private void Awake()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        int quality = PlayerPrefs.GetInt(PrefsKeys.Quality, 2);
        QualitySettings.SetQualityLevel(quality);

        bool isFullscreen = PlayerPrefs.GetInt(PrefsKeys.Fullscreen, 1) == 1;
        Screen.fullScreen = isFullscreen;

        

        resolutions = Screen.resolutions;

        // Если сохранённого разрешения нет, используем текущее разрешение монитора
        int width = PlayerPrefs.GetInt(PrefsKeys.ResolutionWidth, Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt(PrefsKeys.ResolutionHeight, Screen.currentResolution.height);

        // По умолчанию оставляем текущее разрешение, если сохранённое не найдено среди доступных
        Resolution target = Screen.currentResolution;

        foreach (Resolution res in resolutions)
        {
            if(res.width == width && res.height == height)
            {
                target = res;
                break;
            }
        }
        
        Screen.SetResolution(target.width, target.height, Screen.fullScreen);

    }
}
