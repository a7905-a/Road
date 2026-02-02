using UnityEngine;

public class ChaseState : IEnemyState
{
    public void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("[Chase State] : State Entered");
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Chase State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            enemy.TransitionToState(new AttackState());
        }
    }

}
