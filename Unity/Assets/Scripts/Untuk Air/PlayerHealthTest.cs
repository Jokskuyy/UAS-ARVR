using UnityEngine;

/// <summary>
/// SCRIPT TESTING (HANYA UNTUK DEBUG).
/// Bertindak seolah-olah menjadi Player untuk mengetes apakah sistem bahaya air bekerja.
/// </summary>
public class PlayerHealthTest : MonoBehaviour
{
    [Header("Simulasi Darah")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Damage Settings")]
    public float electricDamage = 10f; // Damage kena setrum
    public float toxicDamage = 5f;     // Damage kena racun

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("[TESTER] Player Dummy Siap. Menunggu bahaya air...");
    }

    void Update()
    {
        // 1. BACA DATA DARI SCRIPT HAZARD (Ini cara integrasinya nanti)
        int listrikAktif = HazardItem.ActiveElectricHazards;
        int racunAktif = HazardItem.ActiveToxicHazards;

        // bool isHurt = false;

        // 2. LOGIKA KENA DAMAGE
        if (listrikAktif > 0)
        {
            float damage = electricDamage * listrikAktif * Time.deltaTime;
            currentHealth -= damage;
            // isHurt = true;
            // Debug.Log untuk melihat efeknya
            Debug.Log($"<color=yellow>[KESETRUM]</color> Ada {listrikAktif} sumber listrik! HP: {(int)currentHealth}");
        }

        if (racunAktif > 0)
        {
            float damage = toxicDamage * racunAktif * Time.deltaTime;
            currentHealth -= damage;
            // isHurt = true;
            Debug.Log($"<color=green>[KERACUNAN]</color> Ada {racunAktif} sumber racun! HP: {(int)currentHealth}");
        }

        // 3. LOGIKA MATI
        if (currentHealth <= 0)
        {
            Debug.LogError("GAME OVER: Player Mati dalam simulasi!");
            currentHealth = 100f; // Reset biar bisa tes lagi
        }
    }
}