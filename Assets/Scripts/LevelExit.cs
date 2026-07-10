using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0.5f;
    [SerializeField] AudioClip exitSound;
    
    // при контакте с тригером запускаем следующую сцену
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        AudioSettings.instance.PlaySoundEffect(exitSound);
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        // делаем сброс обьекта ScenePersist при переходе на следущую сцену
        FindAnyObjectByType<ScenePersist>().ResetScenePersist();
        // загрузка следующий сцены
        SceneManager.LoadScene(nextSceneIndex);
    }
}
