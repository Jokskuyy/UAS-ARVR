using UnityEngine;

public class FinishScript : MonoBehaviour
{
    [Header("Finish Settings")]
    [Tooltip("Tag untuk item yang harus diselamatkan")]
    public string itemTag = "Item";
    [Tooltip("Tag untuk NPC yang harus diselamatkan")]
    public string npcTag = "NPC";
    [Tooltip("Tampilkan pesan debug saat cek kondisi finish")]
    public bool showDebugLogs = true;

    private bool hasTriggered = false;
    private bool playerInFinishZone = false;

    private void Update()
    {
        // Cek terus-menerus jika player ada di finish zone
        if (playerInFinishZone && !hasTriggered)
        {
            CheckAndTriggerFinish();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk adalah Player
        if (other.CompareTag("Player"))
        {
            playerInFinishZone = true;
            
            if (showDebugLogs)
                Debug.Log("Player masuk ke Finish Zone!");
            
            CheckAndTriggerFinish();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player keluar dari finish zone
        if (other.CompareTag("Player"))
        {
            playerInFinishZone = false;
            
            if (showDebugLogs)
                Debug.Log("Player keluar dari Finish Zone!");
        }
    }

    private void CheckAndTriggerFinish()
    {
        if (hasTriggered) return;

        // Cek apakah masih ada Item yang belum diselamatkan
        GameObject[] remainingItems = GameObject.FindGameObjectsWithTag(itemTag);
        GameObject[] remainingNPCs = GameObject.FindGameObjectsWithTag(npcTag);

        int itemCount = remainingItems.Length;
        int npcCount = remainingNPCs.Length;

        if (showDebugLogs)
        {
            Debug.Log($"[FINISH CHECK] Item tersisa: {itemCount}, NPC tersisa: {npcCount}");
            
            // Debug detail: Tampilkan nama-nama item yang masih tersisa
            if (itemCount > 0)
            {
                string itemList = "=== ITEM YANG MASIH TERSISA ===\n";
                for (int i = 0; i < remainingItems.Length; i++)
                {
                    itemList += $"{i + 1}. {remainingItems[i].name} (Posisi: {remainingItems[i].transform.position})\n";
                }
                Debug.Log(itemList);
            }
            
            // Debug detail: Tampilkan nama-nama NPC yang masih tersisa
            if (npcCount > 0)
            {
                string npcList = "=== NPC YANG MASIH TERSISA ===\n";
                for (int i = 0; i < remainingNPCs.Length; i++)
                {
                    npcList += $"{i + 1}. {remainingNPCs[i].name} (Posisi: {remainingNPCs[i].transform.position})\n";
                }
                Debug.Log(npcList);
            }
        }

        // Jika masih ada item atau NPC yang belum diselamatkan
        if (itemCount > 0 || npcCount > 0)
        {
            if (showDebugLogs)
            {
                string message = "Belum bisa finish! Masih ada:\n";
                if (itemCount > 0) message += $"- {itemCount} Item yang belum diselamatkan\n";
                if (npcCount > 0) message += $"- {npcCount} NPC yang belum diselamatkan";
                Debug.LogWarning(message);
            }
            return;
        }

        // Semua item dan NPC sudah diselamatkan, trigger finish!
        TriggerFinish();
    }

    public void TriggerFinish()
    {
        if (hasTriggered) 
        {
            Debug.LogWarning("Finish sudah di-trigger sebelumnya!");
            return;
        }

        hasTriggered = true;
        Debug.Log("Game Selesai!");

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