using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
     
    void Awake()
    {
        // проверка на количество GameSession в сцене
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        // если больше одного - удаляем дубликат
        if(numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // если один - сохранияем между сценами
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath()
    {
        // если жизней больше 1 - перезапускаем сцену, если нет - полностью сбрасываем игру
        if(playerLives > 1)
        {
            Invoke("TakeLife", 1f);
        }
        else
        {
            Invoke("ResetGameSession", 1f);
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

    void ResetGameSession()
    {
        // сбрасываем обьект ScenePersist
        FindAnyObjectByType<ScenePersist>().ResetScenePersist();
        // загрузка стартовой сцены
        SceneManager.LoadScene(0);
        // уничтожение текущей игровой сессии
        Destroy(gameObject);
    }

    // добавление очков и обновление UI
    public void AddPlayerScore(int score)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
    }
    
}
