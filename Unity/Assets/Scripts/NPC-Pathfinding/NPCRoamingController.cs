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

    public bool isBeingCarried = false; 

    void Start()
    {
    
        agent = GetComponent<NavMeshAgent>();

        // Pastikan NPC mulai bergerak
        if (waypoints.Count > 0)
        {
            SetNextDestination();
        }
    }

    void Update()
    {
        if (!isBeingCarried)
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
        else
        {
            agent.isStopped = true;
        }
    }

    private void SetNextDestination()
    {
        if (waypoints.Count == 0) return;

        int newIndex = currentWaypointIndex;
        while (newIndex == currentWaypointIndex)
        {
            newIndex = Random.Range(0, waypoints.Count);
        }
        currentWaypointIndex = newIndex;

        agent.destination = waypoints[currentWaypointIndex].position;
        agent.isStopped = false;
    }
    
    public void StartCarrying(Transform playerTransform)
    {
        isBeingCarried = true;
        
        agent.isStopped = true;
        transform.SetParent(playerTransform); 
    }
public void StopCarrying()
{
  
    isBeingCarried = false;

    transform.SetParent(null); 

    agent.isStopped = false; 
    
    agent.velocity = Vector3.zero; 
    agent.isStopped = true; 
    
    Debug.Log("Nenek berhasil diturunkan.");
}
}