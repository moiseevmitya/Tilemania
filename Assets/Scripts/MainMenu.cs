using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioClip menuMusic;
    
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Замена трека на музыку главного меню + проверка, чтобы не перезапускать музыку
        AudioSettings.instance.SetAndPlayMusic(menuMusic);
    }
    
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Level0");
    }

    public void OpenSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
