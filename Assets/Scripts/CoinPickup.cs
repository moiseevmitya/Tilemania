using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinScore = 100;
    
    // защита от повторного подбора
    bool wasCollected = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // проверка подбора монеты игроком
        if(other.CompareTag("Player") && !wasCollected)
        {
            FindAnyObjectByType<GameSession>().AddPlayerScore(coinScore);
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
