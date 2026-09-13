// Coin.cs - Collectible coin system

using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioManager audioManager;
    private ScoreManager scoreManager;
    private bool collected = false;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        collected = false;
    }

    private void Update()
    {
        // Rotate coin for visual effect
        transform.Rotate(Vector3.up * 180f * Time.deltaTime);
    }

    public void Collect()
    {
        if (collected) return;

        collected = true;
        scoreManager.AddCoins(1);
        audioManager.PlaySFX("coin");
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }
}
