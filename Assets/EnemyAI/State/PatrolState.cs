using UnityEngine;

public class PatrolState : IEnemyState
{
    static readonly int MoveHash = Animator.StringToHash("Move");
    public void EnterState(EnemyStateManager enemy)
    {
        enemy.animator.Play(MoveHash);
        enemy.simplePatrol.MoveToNextPatrolPoint();
    }

    public void ExitState(EnemyStateManager enemy)
    {
        
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        
        if (enemy.attackController.targetToAttack != null)
        {
            enemy.TransitionToState(new ChaseState());
            return;
        }

        
        if (enemy.simplePatrol.HasReachedDes())
        {
            enemy.TransitionToState(new IdleState());
        }
    }
}
