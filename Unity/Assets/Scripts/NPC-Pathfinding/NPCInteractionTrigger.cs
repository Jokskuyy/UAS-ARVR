using UnityEngine;

public class NPCInteractionTrigger : MonoBehaviour
{
    private NPCRoamingController roamingController;
    
    private Transform playerTransform = null; 

    void Start()
    {
        roamingController = GetComponent<NPCRoamingController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // SIMPAN Transform (other.transform)
            playerTransform = other.transform; 
            Debug.Log("Player masuk jangkauan. Tombol Gendong siap!");
        }
    }

    // Dipanggil saat objek lain keluar dari trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!roamingController.isBeingCarried)
            {
                playerTransform = null; 
                Debug.Log("Player keluar jangkauan.");
            }
        }
    }

    void Update()
    {
        // Cek Input Tombol 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (roamingController.isBeingCarried)
            {
                roamingController.StopCarrying();
                
                playerTransform = null;
            }
            else if (playerTransform != null) 
            {
                roamingController.StartCarrying(playerTransform);
            }
        }
    }
}