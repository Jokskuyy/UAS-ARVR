using UnityEngine;

public class FinishScript : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("player"))
        {
            hasTriggered = true;
            Debug.Log("Masuk Finish Line!");

            if (GameManager.Instance != null)
            {
                // Cuma panggil ini, gak perlu ngirim angka apa-apa
                GameManager.Instance.OnGameEnd(); 
            }
        }
    }
}