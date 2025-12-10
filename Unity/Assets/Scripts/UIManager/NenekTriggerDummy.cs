using UnityEngine;
public class NenekTriggerDummy : MonoBehaviour
{
    public bool destroyAfterTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        // --- GANTI DI SINI ---
        if (other.CompareTag("Finish")) 
        {
            if (GameManager.Instance != null) GameManager.Instance.OnGrandmaFound();
            SetWaktuNenekToScoreUI();
            GetComponent<Collider>().enabled = false;
            // Hancurkan object jika diset true
            if (destroyAfterTrigger)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetWaktuNenekToScoreUI()
    {
        if (ScoreUIManager.Instance != null)
        {
            float waktuNenek = Time.timeSinceLevelLoad;
            ScoreUIManager.Instance.SetWaktuNenek(waktuNenek);
            Debug.Log($"Waktu Nenek ditemukan: {waktuNenek:F2} detik");
        }
        else
        {
            Debug.LogWarning("ScoreUIManager.Instance tidak ditemukan!");
        }
    }
}