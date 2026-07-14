using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int scoreForPinkEnemy = 100;
    [SerializeField] private int scoreForYellowEnemy = 150;
    [SerializeField] private AudioClip enemyDeathSound;
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;

    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();

        // задаем направление полета пули
        xSpeed = bulletSpeed * player.transform.localScale.x;
    }

    void Update()
    {
        // постоянное движение пули по x 
        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // проверка на попадание во врага
        if(other.CompareTag("EnemyPink"))
        {
            CallEnemyDeath(other.gameObject, scoreForPinkEnemy);
        }
        else if(other.CompareTag("EnemyYellow"))
        {
            CallEnemyDeath(other.gameObject, scoreForYellowEnemy);
        }
        Destroy(gameObject);
    }

    // уничтожаем пулю при попадании в любой колайдер 
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject, 1f);
    }

    private void CallEnemyDeath(GameObject enemy, int score)
    {
        Animator enemyAnimator = enemy.GetComponent<Animator>();
        // запуск анимации сметри + проверка на налицие аниматора
        enemyAnimator?.SetTrigger("EnemyDying");

        AudioSettings.instance.PlaySoundEffect(enemyDeathSound);

        // удаляем врага
        Destroy(enemy.gameObject, 0.15f);
        // начисляем очки за убийство
        FindAnyObjectByType<GameSession>().AddPlayerScore(score);
    }
}
