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
    
    private void OnDrawGizmos()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] != null)
            {
                Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);

                // 다음 포인트로 선 긋기
                if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
                // 마지막 포인트에서 첫 번째 포인트로 이어주기 (순환 루프 완성)
                else if (i == patrolPoints.Length - 1 && patrolPoints[0] != null && patrolPoints.Length > 1)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
                }
            }
        }
    }


}
