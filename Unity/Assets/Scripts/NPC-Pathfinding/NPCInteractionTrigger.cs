using UnityEngine;

public class NPCInteractionTrigger : MonoBehaviour
{
    private NPCRoamingController roamingController;
    private Transform playerTransform = null;
    private bool playerInRange = false;

    void Start()
    {
        roamingController = GetComponent<NPCRoamingController>();
        if (roamingController == null)
        {
            Debug.LogError("Script NPCRoamingController tidak ditemukan di objek ini!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerInRange = true;
            Debug.Log("Tekan 'E' untuk Ajak Ikut / Suruh Tunggu.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Jika sedang tidak follow, kita lupakan playernya
            if (!roamingController.isFollowing)
            {
                playerTransform = null;
            }
        }
    }

    void Update()
    {
        // Deteksi tombol E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // KASUS 1: Sedang Mengikuti -> Suruh Berhenti
            if (roamingController.isFollowing)
            {
                roamingController.StopFollowing();
                playerTransform = null;
            }
            // KASUS 2: Belum Mengikuti & Player Dekat -> Suruh Ikut
            else if (playerInRange && playerTransform != null)
            {
                roamingController.StartFollowing(playerTransform);
            }
        }
    }
}