using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCRoamingController : MonoBehaviour
{
    [Header("Komponen")]
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Patroli Settings")]
    public List<Transform> waypoints;
    public float patrolTime = 5f;
    private float timer;
    private int currentWaypointIndex = 0;

    [Header("Follow Settings")]
    public bool isFollowing = false;
    public float followDistance = 2.0f;
    private Transform targetTransform; // Target yang diikuti (Kepala Player)

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Cari Animator (di badan sendiri atau di anak object)
        anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        agent.stoppingDistance = 0.5f;

        // Mulai patroli jika ada waypoint
        if (waypoints.Count > 0) SetNextDestination();
    }

    void Update()
    {
        // 1. Selalu update animasi (Jalan/Diam)
        UpdateAnimation();

        // 2. LOGIKA FOLLOW
        if (isFollowing && targetTransform != null)
        {
            MoveBehindTarget();
            return; // Jangan jalankan patroli
        }

        // 3. LOGIKA PATROLI
        PatrolBehavior();
    }

    // --- FUNGSI PUBLIK (Bisa dipanggil dari Script Lain) ---

    public void StartFollowing(Transform target)
    {
        isFollowing = true;
        targetTransform = target;

        agent.enabled = true;
        agent.isStopped = false;
        agent.speed = 4.5f; // Percepat sedikit

        Debug.Log("Controller: Mulai mengikuti target.");
    }

    public void StopFollowing()
    {
        isFollowing = false;
        targetTransform = null;

        agent.speed = 3.5f; // Kecepatan normal

        // Langsung cari waypoint baru
        SetNextDestination();

        Debug.Log("Controller: Berhenti mengikuti.");
    }

    // --- FUNGSI INTERNAL ---

    private void MoveBehindTarget()
    {
        // Ambil arah hadap target (Player)
        Vector3 lookDir = targetTransform.forward;
        lookDir.y = 0; // Ratakan ke tanah (biar gak dongak ke atas)
        lookDir.Normalize();

        // Hitung posisi belakang
        Vector3 dest = targetTransform.position - (lookDir * followDistance);
        agent.destination = dest;
    }

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

    private void UpdateAnimation()
    {
        if (anim != null)
        {
            float speed = agent.velocity.magnitude;
            anim.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
    }
}