using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChaseState : IEnemyState
{
    float chaseTime = 2f;
    float chaseTimer = 0;
    static readonly int ChaseHash = Animator.StringToHash("Chase");
    public void EnterState(EnemyStateManager enemy)
    {
        enemy.animator.Play(ChaseHash);
        
        chaseTimer = 0f;
    }

    public void ExitState(EnemyStateManager enemy)
    {
        
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        chaseTimer += Time.deltaTime;
        if (enemy.attackController.targetToAttack != null)
        {
            chaseTimer = 0;
        }

        if (enemy.attackController.targetToAttack == null && chaseTimer >= chaseTime)
        {
            if (Random.value > 0.5f)
            {
                enemy.TransitionToState(new IdleState());
            }
            else
            {
                enemy.TransitionToState(new PatrolState());
            }
            return;
        }

        if (enemy.attackController.targetToAttack != null)
        {
            enemy.agent.SetDestination(enemy.attackController.targetToAttack.position);

            float dist = Vector3.Distance(enemy.transform.position, enemy.attackController.targetToAttack.position);
            if (dist <= enemy.baseUnit.unitData.AttackRange)
            {
                
                enemy.TransitionToState(new AttackState());
                //공격범위 내 플레이어가 있다면 공격상태로 전환
            }
        }

    }

}
