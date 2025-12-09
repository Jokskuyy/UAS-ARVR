using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NPCRoamingController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim; // Variabel untuk mengontrol animasi

    [Header("Patrol Settings")]
    public List<Transform> waypoints;
    public float patrolTime = 5f;
    private float timer;
    private int currentWaypointIndex = 0;

    [Header("Follow Settings")]
    public bool isFollowing = false;
    public float followDistance = 2.0f; // Jarak berdirinya di belakang player
    private Transform playerTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Mencari komponen Animator di object ini
        anim = GetComponent<Animator>();

        // PENGAMAN: Jika tidak ketemu di body utama, cari di anak-anak object (modelnya)
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        // Setting jarak berhenti agar tidak terlalu nempel
        agent.stoppingDistance = 0.5f;

        if (waypoints.Count > 0)
        {
            SetNextDestination();
        }
    }

    void Update()
    {
        // 1. UPDATE ANIMASI (PENTING!)
        UpdateAnimation();

        // 2. LOGIKA FOLLOW (MENGIKUTI)
        if (isFollowing && playerTarget != null)
        {
            // Rumus: Cari posisi di BELAKANG punggung player
            Vector3 targetPosition = playerTarget.position - (playerTarget.forward * followDistance);
            agent.destination = targetPosition;
            return; // Jangan jalankan logika patroli di bawah
        }

        // 3. LOGIKA PATROLI (JALAN SENDIRI)
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

    // Fungsi khusus kirim sinyal ke Animator
    void UpdateAnimation()
    {
        if (anim != null)
        {
            // Mengambil kecepatan asli dari NavMeshAgent (0 = diam, >0 = jalan)
            float currentSpeed = agent.velocity.magnitude;

            // Kirim angka ini ke Parameter 'Speed' yang ada di Animator
            // 0.1f adalah waktu transisi agar animasi haluss
            anim.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);
        }
    }

    private void SetNextDestination()
    {
        if (waypoints.Count == 0) return;

        // Pengaman jika waypoint cuma 1
        if (waypoints.Count == 1)
        {
            agent.destination = waypoints[0].position;
            return;
        }

        int newIndex = currentWaypointIndex;
        while (newIndex == currentWaypointIndex)
        {
            newIndex = Random.Range(0, waypoints.Count);
        }
        currentWaypointIndex = newIndex;

        agent.destination = waypoints[currentWaypointIndex].position;
        agent.isStopped = false;
    }

    public void StartFollowing(Transform player)
    {
        isFollowing = true;
        playerTarget = player;

        agent.enabled = true;
        agent.isStopped = false;

        // Percepat sedikit saat mengejar player (opsional)
        agent.speed = 4.5f;

        Debug.Log("Nenek mulai mengikuti...");
    }

    public void StopFollowing()
    {
        isFollowing = false;
        playerTarget = null;

        // Kembalikan kecepatan jalan santai
        agent.speed = 3.5f;

        SetNextDestination();
        Debug.Log("Nenek berhenti mengikuti.");
    }
}