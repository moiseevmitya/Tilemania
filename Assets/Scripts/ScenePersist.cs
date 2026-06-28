using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        // проверка на количество ScenePersist в сцене
        int numberScenePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        // если больше одного - удаляем дубликат
        if(numberScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // сохраняем обьект при перезагрузке сцены
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // удаляем обьект
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
