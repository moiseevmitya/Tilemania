using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int playerScore = 0;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AudioClip gameMusic;
     
    void Awake()
    {
        // проверка на количество GameSession в сцене
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        // если больше одного - удаляем дубликат
        if(numberGameSessions > 1)
        {
            Destroy(gameObject);
            return;
        }

        // если один - сохранияем между сценами
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();

        // Замена трека на игровую музыку + проверка, чтобы не перезапускать музыку
        AudioSettings.instance.SetAndPlayMusic(gameMusic);
    }

    public void ProcessPlayerDeath()
    {
        // если жизней больше 1 - перезапускаем сцену, если нет - полностью сбрасываем игру
        if(playerLives > 1)
        {
            Invoke(nameof(TakeLife), 1f);
        }
        else
        {
            Invoke(nameof(ResetGameSession), 1f);
        }
    }

    void TakeLife()
    {
        // отнимаем жизнь
        playerLives--;
        // обновляем UI
        livesText.text = playerLives.ToString();
        // перезапускаем сцену
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetGameSession()
    {
        Time.timeScale = 1f;
        // сбрасываем обьект ScenePersist
        ScenePersist scenePersist = FindAnyObjectByType<ScenePersist>();
        scenePersist?.ResetScenePersist();

        // загрузка стартовой сцены
        SceneManager.LoadSceneAsync("MainMenu");

        // уничтожение текущей игровой сессии
        Destroy(gameObject);
    }

    // добавление очков и обновление UI
    public void AddPlayerScore(int score)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }
    
}
