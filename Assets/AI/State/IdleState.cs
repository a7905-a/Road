using UnityEngine;

public class IdleState : IEnemyState
{
    float idleTime;
    float timer;

    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("Idle");
        idleTime = Random.Range(1f, 4f);
        timer = 0f;
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Idle State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        //  if (enemy.GetComponent<EnemySight>().IsPlayerInSight())
        // {
        //     enemy.TransitionToState(new ChaseState());
        //     return;
        // }

        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            enemy.TransitionToState(new PatrolState());
            return;
        }
    }
}
