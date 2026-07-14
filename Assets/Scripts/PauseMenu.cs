using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private AudioClip openSound;
    public static bool isPaused {get; private set;}

    private void Awake()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        CursorController.Hide();
    }
    
    private void OnEnable()
    {
        pauseAction.action.performed += OnPausePerformed;
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPausePerformed;
        pauseAction.action.Disable();

        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void BackToMainMenu()
    {
        isPaused = false;

        Time.timeScale = 1f;

        CursorController.Show();

        GameSession gameSession = FindAnyObjectByType<GameSession>();

        if (gameSession != null)
        {
            gameSession.ResetGameSession();
        }
        else
        {
            SceneManager.LoadSceneAsync(mainMenuSceneName);
        }
    }

    public void Resume()
    {
        isPaused = false;

        pausePanel.SetActive(false);
        
        Time.timeScale = 1f;

        CursorController.Hide();
    }

    public void RestartLevel()
    {
        isPaused = false;

        Time.timeScale = 1f;

        CursorController.Hide();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        isPaused = true;
        
        pausePanel.SetActive(true);

        Time.timeScale = 0f;

        CursorController.Show();

        AudioSettings.instance?.PlaySoundEffect(openSound);
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if(isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}
