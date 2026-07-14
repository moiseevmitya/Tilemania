using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;
    [SerializeField] private int coinScore = 100;
    
    // защита от повторного подбора
    bool wasCollected = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // проверка подбора монеты игроком
        if(other.CompareTag("Player") && !wasCollected)
        {
            FindAnyObjectByType<GameSession>().AddPlayerScore(coinScore);
            wasCollected = true;
            AudioSettings.instance.PlaySoundEffect(coinPickupSFX);
            Destroy(gameObject);
        }
    }
}
