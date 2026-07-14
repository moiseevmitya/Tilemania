using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // постоянное движение по x
        myRigidbody.linearVelocity = new Vector2(moveSpeed, 0f);
    }


    // при столконовении с колайдером меняем направление движение и поворачиваем врага по x 
    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        flipEnemy();
    }

    private void flipEnemy()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
}
