using UnityEngine;

public class FinishScript : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            TriggerFinish();
        }
    }

    public void TriggerFinish()
    {
        if (hasTriggered) 
        {
            Debug.LogWarning("Finish sudah di-trigger sebelumnya!");
            return;
        }

        hasTriggered = true;
        Debug.Log("Masuk Finish Line!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameEnd(); 
        }
        else
        {
            Debug.LogWarning("GameManager.Instance tidak ditemukan!");
        }
    }
}