using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 0.5f;
    [SerializeField] private AudioClip exitSound;

    private bool isLoading;
    
    // при контакте игрока с тригером запускаем следующую сцену
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player") || isLoading)
        {
            return;
        }
        
        isLoading = true;
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        AudioSettings.instance?.PlaySoundEffect(exitSound);

        yield return new WaitForSeconds(levelLoadDelay);

        // делаем сброс обьекта ScenePersist при переходе на следущую сцену
        ScenePersist scenePersist = FindAnyObjectByType<ScenePersist>();
        scenePersist?.ResetScenePersist();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // загрузка следующий сцены
        SceneManager.LoadScene(nextSceneIndex);
    }
}
