using UnityEngine;
using UnityEngine.AI;

public class SimplePatrol : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPointIndex].position);
        agent.isStopped = false;

        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }


    public bool HasReachedDes()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }

}
