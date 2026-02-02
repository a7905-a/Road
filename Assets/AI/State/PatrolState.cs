using UnityEngine;

public class PatrolState : IEnemyState
{
    public void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("[Patrol State] : State Entered");
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Patrol State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            enemy.TransitionToState(new ChaseState());
        }
    }
}
