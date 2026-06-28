using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
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
        if(other.CompareTag("Enemy"))
        {
            Animator enemyAnimator = other.GetComponent<Animator>();
            // запуск анимации сметри + проверка на налицие аниматора
            enemyAnimator?.SetTrigger("EnemyDying");
            // удаляем врага
            Destroy(other.gameObject, 0.15f);
        }
        Destroy(gameObject);
    }

    // уничтожаем пулю при попадании в любой колайдер 
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject, 1f);
    }
}
