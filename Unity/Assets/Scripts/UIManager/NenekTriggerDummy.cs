using UnityEngine;
public class NenekTriggerDummy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // --- GANTI DI SINI ---
        if (other.CompareTag("player")) 
        {
            if (GameManager.Instance != null) GameManager.Instance.OnGrandmaFound();
            if (ScoreUIManager.Instance != null) ScoreUIManager.Instance.SetWaktuNenek(Time.timeSinceLevelLoad);
            GetComponent<Collider>().enabled = false;
        }
    }
}