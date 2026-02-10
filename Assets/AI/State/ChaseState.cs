using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChaseState : IEnemyState
{
    float chaseTime = 2f;
    float chaseTimer = 0;
    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("chase");
        chaseTimer = 0f;
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Chase State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        chaseTimer += Time.deltaTime;
        //Transform player = enemy.Player;

        // bool playerVisible = enemy.GetComponent<EnemySight>().IsPlayerInSight();
        // if (playerVisible)
        // {
        //     chaseTimer = 0f; 
        // } 
            

        // if(!playerVisible && chaseTimer >= chaseTime)
        // {
        //     if (Random.value > 0.5f)
        //     {
        //         enemy.TransitionToState(new IdleState());
        //     }
        //     else
        //     {
        //         enemy.TransitionToState(new PatrolState());
        //     }
        //     return;
        // }

        // if (player != null)
        // {
        //     Vector3 dir = (player.position - enemy.transform.position).normalized;
        //     enemy.rb.velocity = new Vector3(dir.x * enemy.GetComponent<EnemyDateManger>().enemyDate.ChaseSpeed, 0f);

        //     if (dir.x != 0)
        //     {
        //         Vector3 scale = enemy.transform.localScale;
        //         Scale.s = -Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
        //         enemy.transform.localScale = scale;
        //     }

        //     float dist = Vector3.Distance(enemy.transform.position, player.position);
        //     if (dist <= enemy.GetComponent<EnemyDateManger>().enemyDate.AttackRange)
        //     {
        //         enemy.rb.velocity = Vector3.zero;
        //         enemy.TransitionToState(new AttackState());
        //         //공격범위 내 플레이어가 있다면 공격상태로 전환
        //     }
        // }
    }

}
