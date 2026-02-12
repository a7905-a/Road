using UnityEngine;

public class IdleState : IEnemyState
{
    float idleTime;
    float timer;
    static readonly int IdleHash = Animator.StringToHash("Idle");

    public void EnterState(EnemyStateManager enemy)
    {
        enemy.animator.Play(IdleHash);
        idleTime = Random.Range(1f, 4f);
        timer = 0f;
    }

    public void ExitState(EnemyStateManager enemy)
    {
        
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.currentTarget != null)
        {
            enemy.TransitionToState(new ChaseState());
            return;
        }

        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            enemy.TransitionToState(new PatrolState());
            return;
        }
    }
}
