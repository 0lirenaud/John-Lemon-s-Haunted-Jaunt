using StealthGame;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WaypointPatrol : MonoBehaviour
{
    const float REGULAR_STOP_DISTANCE = 0.2f;
    const float ALERT_STOP_DISTANCE = 1f;

    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;

    GameObject player;
    GhostBehaviour ghostBehaviour;
    int m_CurrentWaypointIndex;
    [SerializeField]
    bool isAlerted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("JohnLemon");
        ghostBehaviour = GetComponent<GhostBehaviour>();
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (ghostBehaviour.isChasing)
        {
            navMeshAgent.SetDestination(player.transform.position);
            isAlerted = false;
        } 
        else if (isAlerted)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                isAlerted = false;
            }
        } 
        else
        {
            navMeshAgent.stoppingDistance = REGULAR_STOP_DISTANCE;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    public void GoToAlert(Vector3 position)
    {
        if (!ghostBehaviour.isChasing)
        {
            isAlerted = true;
            navMeshAgent.SetDestination(position);
            navMeshAgent.stoppingDistance = ALERT_STOP_DISTANCE;
        }
    }
}
