using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioClip openSound;

    void OnEnable()
    {
        AudioSettings.instance.PlaySoundEffect(openSound);
        Time.timeScale = 0;
        ShowCursor();
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        HideCursor();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        GameSession gameSession = FindAnyObjectByType<GameSession>();
        gameSession.ResetGameSession();
        ShowCursor();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        HideCursor();
        transform.parent.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        HideCursor();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
