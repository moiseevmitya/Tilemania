using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private AudioClip endGameMusic;
    
    void Start()
    {
        Time.timeScale = 1f;
        CursorController.Show();
        

        int currentPlayerScore = 0;
        int bestPlayerScore = PlayerPrefs.GetInt(PrefsKeys.BestPlayerScore, 0);

        GameSession gameSession = FindAnyObjectByType<GameSession>();

        if(gameSession != null)
        {
            currentPlayerScore = gameSession.GetPlayerScore();

            // проверка на лучший счет
            if(currentPlayerScore > bestPlayerScore)
            {
                bestPlayerScore = currentPlayerScore;
                PlayerPrefs.SetInt(PrefsKeys.BestPlayerScore, bestPlayerScore);

                PlayerPrefs.Save();
            }

            // очищаем игровую сессию
            Destroy(gameSession.gameObject);
        }

        bestScore.text = bestPlayerScore.ToString();
        currentScore.text = currentPlayerScore.ToString();

        // Замена трека на финальную музыку
        AudioSettings.instance?.SetAndPlayMusic(endGameMusic);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
