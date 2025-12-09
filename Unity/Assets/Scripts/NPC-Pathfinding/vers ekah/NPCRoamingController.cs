using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NPCRoamingController : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Transform> waypoints;

    private int currentWaypointIndex = 0;
    public float patrolTime = 5f;
    private float timer;

    // --- VARIABEL UNTUK FOLLOW ---
    public bool isFollowing = false;
    private Transform playerTarget;

    [Tooltip("Jarak NPC berdiri di belakang pemain (meter)")]
    public float followDistance = 2.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Kita set stopping distance kecil saja, 
        // karena kita menargetkan titik SPESIFIK di belakang player, bukan player-nya.
        agent.stoppingDistance = 0.5f;

        if (waypoints.Count > 0)
        {
            SetNextDestination();
        }
    }

    void Update()
    {
        // --- LOGIKA MENGIKUTI PLAYER (DIBELAKANG) ---
        if (isFollowing && playerTarget != null)
        {
            // RUMUS: Posisi Player dikurangi (Arah Depan * Jarak) = Posisi Belakang
            Vector3 targetPosition = playerTarget.position - (playerTarget.forward * followDistance);

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

    private void SetNextDestination()
    {
        if (waypoints.Count == 0) return;

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

        // Update speed jika perlu, biar bisa ngejar kalau player lari
        agent.speed = 4.5f; // Sesuaikan dengan kecepatan player Anda

        Debug.Log("Nenek mulai mengikuti di belakang Player...");
    }

    public void StopFollowing()
    {
        isFollowing = false;
        playerTarget = null;

        // Reset speed ke jalan santai
        agent.speed = 3.5f;

        SetNextDestination();
        Debug.Log("Nenek berhenti mengikuti.");
    }
}