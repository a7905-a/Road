using UnityEngine;

public class AttackState : IEnemyState
{
    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("Attack");
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Attack State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        AnimatorStateInfo stateInfo = enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        // if(!stateInfo.IsName("Attack"))
        // {
        //     if (enemy.GetComponent<EnemySight>().IsPlayerInSight())
        //     {
        //         enemy.TransitionToState(new ChaseState());
        //         return;
        //     }
        //     else
        //     {
        //         if (Random.value < 0.5f)
        //         {
        //             enemy.TransitionToState(new IdleState());
        //         }
        //         else
        //         {
        //             enemy.TransitionToState(new PatrolState());
        //         }
        //         return;
        //     }
            
        // }
    }
}
