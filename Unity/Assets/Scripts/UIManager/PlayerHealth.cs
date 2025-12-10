using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Status Pemain")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false; // Penanda biar gak mati berkali-kali

    private void Start()
    {
        currentHealth = maxHealth;
        
        // Update UI awal biar penuh
        if (ScoreUIManager.Instance != null) 
        {
            ScoreUIManager.Instance.UpdateHealth(currentHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; // Keamanan ganda

        currentHealth -= amount;
        
        // Pastikan darah tidak minus (Minimal 0)
        if (currentHealth < 0) currentHealth = 0;

        // Update Slider Merah di Layar
        if (ScoreUIManager.Instance != null)
        {
            ScoreUIManager.Instance.UpdateHealth(currentHealth);
        }

        // --- LOGIKA GAME OVER (DARAH HABIS) ---
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("GAME OVER: Player Mati kehabisan darah!");

        // 1. Matikan Gerakan Player (Opsional, biar gak bisa jalan lagi)
        // Cari script gerakan di object ini dan matikan
        GerakanPlayer1 gerakan = GetComponent<GerakanPlayer1>();
        if (gerakan != null)
        {
            gerakan.enabled = false;
        }

        // 2. PANGGIL GAME MANAGER UNTUK KIRIM DATA
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameEnd();
        }
    }
}