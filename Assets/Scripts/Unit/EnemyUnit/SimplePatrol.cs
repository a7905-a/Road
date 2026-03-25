using UnityEngine;
using UnityEngine.AI;

public class SimplePatrol : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    NavMeshAgent agent;
    int currentPointIndex = 0;
    void Awake()
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
