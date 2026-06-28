using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0.5f;
    
    // при контакте с тригером запускаем следующую сцену
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        // проверка на наличие следующей сцены, если сцена посленяя - возращаемся на стартовую сцену
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        // делаем сброс обьекта ScenePersist при переходе на следущую сцену
        FindAnyObjectByType<ScenePersist>().ResetScenePersist();
        // загрузка следующий сцены
        SceneManager.LoadScene(nextSceneIndex);
    }
}
