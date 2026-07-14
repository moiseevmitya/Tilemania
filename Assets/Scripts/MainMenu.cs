using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Level0";
    [SerializeField] private string settingsSceneName = "SettingsMenu";
    
    [SerializeField] private AudioClip menuMusic;
    
    private void OnEnable()
    {
        Time.timeScale = 1f;

        CursorController.Show();
    }
    
    private void Start()
    {
        // Замена трека на музыку главного меню + проверка, чтобы не перезапускать музыку
        if(AudioSettings.instance != null && AudioSettings.instance.musicAudio.clip != menuMusic)
        {
            AudioSettings.instance.musicAudio.clip = menuMusic;
            AudioSettings.instance.musicAudio.Play();
        }
    }
    
    public void StartGame()
    {
        CursorController.Hide();
        SceneManager.LoadSceneAsync(gameSceneName);
    }

    public void OpenSettingsMenu()
    {
        CursorController.Show();
        SceneManager.LoadScene(settingsSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
