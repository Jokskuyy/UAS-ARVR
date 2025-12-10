using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit; 
// Jika Anda pakai XR Toolkit versi terbaru (3.x), biarkan baris di atas. 
// Jika error "Interactables not found", hapus ".Interactables" di bawah.

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))] 
public class VRNPCManager : MonoBehaviour
{
    [Header("Komponen Utama")]
    private NavMeshAgent agent;
    private Animator anim;
    // Referensi ke komponen interaksi VR
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactableVR;

    [Header("Patroli Settings")]
    public List<Transform> waypoints;
    public float patrolTime = 5f;
    private float timer;
    private int currentWaypointIndex = 0;

    [Header("Follow Settings")]
    public bool isFollowing = false;
    public float followDistance = 2.0f;
    private Transform headTarget; // Target kepala (HMD) pemain

    void Start()
    {
        // 1. Setup Komponen Otomatis
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        interactableVR = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        // PENGAMAN: Cari animator di anak jika di parent kosong
        if (anim == null) anim = GetComponentInChildren<Animator>();

        // 2. Setup Agent
        agent.stoppingDistance = 0.5f;

        // 3. Setup Interaksi VR (Otomatis Subscribe Event)
        if (interactableVR != null)
        {
            // Saat NPC "Ditekan/Di-Select" oleh tangan VR, panggil fungsi Interaksi
            interactableVR.selectEntered.AddListener(OnVRInteract);
        }

        // 4. Mulai Patroli
        if (waypoints.Count > 0) SetNextDestination();
    }

    void Update()
    {
        // Update Animasi Jalan/Diam setiap frame
        UpdateAnimation();

        // --- LOGIKA FOLLOW (MENGIKUTI) ---
        if (isFollowing && headTarget != null)
        {
            MoveToBehindPlayer();
            return; // Stop, jangan jalankan patroli kalau lagi ikut
        }

        // --- LOGIKA PATROLI (JALAN SENDIRI) ---
        PatrolBehavior();
    }

    // --- KUMPULAN FUNGSI (MODULAR) ---

    // 1. Fungsi Utama Interaksi (Dipanggil saat Trigger ditekan)
    public void OnVRInteract(SelectEnterEventArgs args)
    {
        Debug.Log("VR Interaksi Terdeteksi!");
        
        // Logika Toggle: Kalau lagi ikut -> Suruh Berhenti. Kalau diam -> Suruh Ikut.
        if (isFollowing)
        {
            StopFollowing();
        }
        else
        {
            StartFollowing();
        }
    }

    // 2. Fungsi Mulai Mengikuti
    public void StartFollowing()
    {
        // Cari Kepala Player (Main Camera) secara otomatis
        if (Camera.main != null)
        {
            headTarget = Camera.main.transform;
            isFollowing = true;
            
            agent.enabled = true;
            agent.isStopped = false;
            agent.speed = 4.5f; // Lari kecil saat mengejar

            Debug.Log("Status: NPC Mengikuti Player.");
        }
        else
        {
            Debug.LogError("ERROR: Main Camera tidak ditemukan! Pastikan XR Origin punya kamera dengan Tag 'MainCamera'.");
        }
    }

    // 3. Fungsi Berhenti Mengikuti
    public void StopFollowing()
    {
        isFollowing = false;
        headTarget = null;
        
        agent.speed = 3.5f; // Jalan santai
        
        // Langsung cari waypoint terdekat untuk lanjut patroli
        SetNextDestination();
        
        Debug.Log("Status: NPC Kembali Patroli.");
    }

    // 4. Logika Pergerakan di Belakang Player (Khusus VR)
    private void MoveToBehindPlayer()
    {
        // Ambil arah pandang player
        Vector3 playerLookDir = headTarget.forward;
        
        // PENTING UNTUK VR: Matikan sumbu Y (atas/bawah). 
        // Agar kalau player nunduk/dongak, NPC tidak mencoba masuk tanah/terbang.
        playerLookDir.y = 0; 
        playerLookDir.Normalize();

        // Hitung posisi di belakang punggung
        Vector3 targetPos = headTarget.position - (playerLookDir * followDistance);
        
        agent.destination = targetPos;
    }

    // 5. Logika Patroli
    private void PatrolBehavior()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer += Time.deltaTime;
            if (timer >= patrolTime)
            {
                SetNextDestination();
                timer = 0f;
            }
        }
    }

    // 6. Logika Ganti Tujuan Patroli
    private void SetNextDestination()
    {
        if (waypoints.Count == 0) return;
        if (waypoints.Count == 1) { agent.destination = waypoints[0].position; return; }

        int newIndex = currentWaypointIndex;
        while (newIndex == currentWaypointIndex)
        {
            newIndex = Random.Range(0, waypoints.Count);
        }
        currentWaypointIndex = newIndex;

        agent.destination = waypoints[currentWaypointIndex].position;
        agent.isStopped = false;
    }

    // 7. Update Animasi ke Animator
    private void UpdateAnimation()
    {
        if (anim != null)
        {
            float speed = agent.velocity.magnitude;
            anim.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
    }

    // Membersihkan Event saat script mati (Biar tidak error memory leak)
    private void OnDestroy()
    {
        if (interactableVR != null)
        {
            interactableVR.selectEntered.RemoveListener(OnVRInteract);
        }
    }
}
