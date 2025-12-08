using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Status Pemain")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false; // Penanda biar gak mati berkali-kali

    [Header("Setting Kerusakan")]
    public float electricDamage = 5f; // Damage per detik per sumber listrik
    public float toxicDamage = 2f;    // Damage per detik per sumber racun

    private void Start()
    {
        currentHealth = maxHealth;
        
        // Update UI awal biar penuh
        if (ScoreUIManager.Instance != null) 
        {
            ScoreUIManager.Instance.UpdateHealth(currentHealth);
        }
    }

    private void Update()
    {
        // Kalau sudah mati, jangan hitung damage lagi
        if (isDead) return;

        int listrikAktif = HazardItem.ActiveElectricHazards;
        int racunAktif = HazardItem.ActiveToxicHazards;

        float totalDamage = 0f;

        // 2. HITUNG DAMAGE
        if (listrikAktif > 0)
        {
            totalDamage += electricDamage * listrikAktif * Time.deltaTime;
        }

        if (racunAktif > 0)
        {
            totalDamage += toxicDamage * racunAktif * Time.deltaTime;
        }

        // 3. TERAPKAN KE DARAH
        if (totalDamage > 0)
        {
            TakeDamage(totalDamage);
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