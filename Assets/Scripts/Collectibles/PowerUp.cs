// PowerUp.cs - Power-up system with multiple types

using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Shield, Magnet, SpeedBoost, Invincibility }

    [SerializeField] private PowerUpType type = PowerUpType.Shield;
    private AudioManager audioManager;
    private ScoreManager scoreManager;
    private bool activated = false;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        activated = false;
    }

    private void Update()
    {
        // Bob up and down
        transform.position += Vector3.up * Mathf.Sin(Time.time * 2f) * Time.deltaTime;
        // Rotate
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    public void Activate(PlayerController player)
    {
        if (activated) return;

        activated = true;
        audioManager.PlaySFX("powerUp");
        scoreManager.AddCoins(5); // Bonus points

        switch (type)
        {
            case PowerUpType.Shield:
                player.ActivateShield();
                break;
            case PowerUpType.Invincibility:
                player.ActivateInvincibility();
                break;
            case PowerUpType.Magnet:
                // Attract nearby coins
                AttractNearbyCoins();
                break;
            case PowerUpType.SpeedBoost:
                // Increase player speed temporarily
                break;
        }

        gameObject.SetActive(false);
    }

    private void AttractNearbyCoins()
    {
        Coin[] coins = FindObjectsOfType<Coin>();
        foreach (Coin coin in coins)
        {
            if (Vector3.Distance(coin.transform.position, transform.position) < 20f)
            {
                coin.Collect();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
                Activate(player);
        }
    }
}
