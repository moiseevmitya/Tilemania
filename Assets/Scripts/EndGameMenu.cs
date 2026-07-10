using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] TextMeshProUGUI currentScore;
    [SerializeField] AudioClip endGameMusic;
    GameSession gameSession;
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        gameSession = FindAnyObjectByType<GameSession>();
        if(gameSession != null)
        {
            int bestPlayerScore = PlayerPrefs.GetInt(PrefsKeys.BestPlayerScore, 0);
            int currentPlayerScore = gameSession.GetPlayerScore();

            // проверка на лучший счет
            if(currentPlayerScore > bestPlayerScore)
            {
                PlayerPrefs.SetInt(PrefsKeys.BestPlayerScore, currentPlayerScore);
                bestPlayerScore = currentPlayerScore;
            }

            bestScore.text = bestPlayerScore.ToString();
            currentScore.text = currentPlayerScore.ToString();
        }

        // очищаем игровую сессию
        Destroy(gameSession.gameObject);

        // Замена трека на финальную музыку
        AudioSettings.instance.SetAndPlayMusic(endGameMusic);
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
