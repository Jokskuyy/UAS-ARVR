using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;
    private bool isInWater = false;
    private PlayerHealth playerHealth;

    void Start()
    {
        // Cari PlayerHealth component di player object
        // Asumsi script ini ada di player object, atau bisa cari dengan tag
        playerHealth = GetComponent<PlayerHealth>();
        
        // Jika tidak ada di object yang sama, cari dengan tag
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("HazardDamage: PlayerHealth tidak ditemukan!");
        }
    }

    void Update()
    {
        if (isInWater && playerHealth != null)
        {
            // Gunakan damagePerSecond * Time.deltaTime untuk damage yang konsisten
            playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    public void SetWater(bool status)
    {
        isInWater = status;
    }
}
