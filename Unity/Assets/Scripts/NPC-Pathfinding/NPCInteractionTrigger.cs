using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// Gunakan namespace ini untuk XR Toolkit terbaru (Unity 6 / XR Toolkit 3.x)
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class NPCInteractionTrigger : MonoBehaviour
{
    private NPCRoamingController roamingController;
    private XRSimpleInteractable interactable;

    void Start()
    {
        // 1. Cari Script Controller di object yang sama
        roamingController = GetComponent<NPCRoamingController>();
        if (roamingController == null)
        {
            Debug.LogError("ERROR: Script 'NPCRoamingController' tidak ditemukan di object ini!");
        }

        // 2. Setup Event VR
        interactable = GetComponent<XRSimpleInteractable>();

        // Mendaftarkan fungsi OnVRInteract agar dipanggil saat 'Select' (Grip/Trigger) ditekan
        interactable.selectEntered.AddListener(OnVRInteract);
    }

    // Fungsi ini otomatis dipanggil oleh XR Toolkit
    private void OnVRInteract(SelectEnterEventArgs args)
    {
        if (roamingController == null) return;

        Debug.Log("Input VR Diterima!");

        // LOGIKA SWITCH (Toggle)
        if (roamingController.isFollowing)
        {
            // Jika sedang ikut -> Suruh Berhenti
            roamingController.StopFollowing();
        }
        else
        {
            // Jika sedang diam -> Cari Kamera Player -> Suruh Ikut
            if (Camera.main != null)
            {
                roamingController.StartFollowing(Camera.main.transform);
            }
            else
            {
                Debug.LogError("Main Camera tidak ditemukan! Pastikan XR Origin punya kamera dengan tag 'MainCamera'.");
            }
        }
    }

    // Bersih-bersih event saat object hancur
    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnVRInteract);
        }
    }
}