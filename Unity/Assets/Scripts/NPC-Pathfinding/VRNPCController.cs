using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
// Wajib ada untuk VR XR Toolkit
using UnityEngine.XR.Interaction.Toolkit;

public class VRNPCController : MonoBehaviour
{
    [Header("Setup Komponen")]
    private NavMeshAgent agent;
    private Animator anim;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable; // Komponen interaksi VR

    [Header("Patroli Settings")]
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    public float patrolTime = 5f;
    private float timer;

    [Header("Follow Settings")]
    public bool isFollowing = false;
    public Transform playerTarget; // Nanti diisi otomatis dengan Kepala (Camera) Player

    [Tooltip("Jarak NPC berdiri di belakang pemain")]
    public float followDistance = 2.0f;

    void Start()
    {
        // 1. Ambil Komponen NavMesh & Animator
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        // 2. Setup Interaksi VR
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable == null)
        {
            Debug.LogError("LUPA PASANG! Tambahkan component 'XR Simple Interactable' di NPC ini.");
        }
        else
        {
            // Mendaftarkan fungsi OnSelectEntered agar terpanggil saat trigger ditekan
            interactable.selectEntered.AddListener(OnSelectEntered);
        }

        // 3. Setup Agent
        agent.stoppingDistance = 0.5f;

        // Mulai Patroli
        if (waypoints.Count > 0) SetNextDestination();
    }

    // Fungsi ini terpanggil otomatis oleh XR Toolkit saat tangan VR menekan tombol Select (Grip/Trigger)
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Toggle Logika (Kalau ikut -> berhenti, Kalau diam -> ikut)
        if (isFollowing)
        {
            StopFollowing();
        }
        else
        {
            // Kita cari object "Main Camera" (Kepala Player) sebagai target
            if (Camera.main != null)
            {
                StartFollowing(Camera.main.transform);
            }
            else
            {
                Debug.LogError("Main Camera tidak ditemukan! Pastikan XR Origin punya kamera dengan tag MainCamera");
            }
        }
    }

    void Update()
    {
        UpdateAnimation();

        // --- LOGIKA FOLLOW VR ---
        if (isFollowing && playerTarget != null)
        {
            // PENTING UNTUK VR:
            // Kita ambil arah hadap kepala player (Forward), TAPI kita hilangkan sumbu Y-nya.
            // Supaya kalau player mendongak ke langit, NPC tidak mencoba terbang/masuk tanah.

            Vector3 playerLookDir = playerTarget.forward;
            playerLookDir.y = 0; // Ratakan ke tanah
            playerLookDir.Normalize();

            // Hitung posisi di belakang punggung
            Vector3 targetPosition = playerTarget.position - (playerLookDir * followDistance);

            agent.destination = targetPosition;
            return;
        }

        // --- LOGIKA PATROLI ---
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

    void UpdateAnimation()
    {
        if (anim != null)
        {
            float currentSpeed = agent.velocity.magnitude;
            anim.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);
        }
    }

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

    public void StartFollowing(Transform target)
    {
        isFollowing = true;
        playerTarget = target;
        agent.enabled = true;
        agent.speed = 4.5f; // Lebih cepat saat ngikut
        Debug.Log("VR: Nenek mulai mengikuti player.");
    }

    public void StopFollowing()
    {
        isFollowing = false;
        playerTarget = null;
        agent.speed = 3.5f; // Santai lagi
        SetNextDestination();
        Debug.Log("VR: Nenek berhenti mengikuti.");
    }

    // Membersihkan event listener saat object hancur/pindah scene
    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }
}