using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    BaseUnit baseUnit;
    Move move;
    float attackTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        baseUnit = animator.GetComponent<BaseUnit>();
        move = animator.GetComponent<Move>();

        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (move.isCommandedMove == true)
        {
        //공격 상태 즉시 해제
        animator.SetBool("Attack", false);
        
        //이동 상태 즉시 활성화
        animator.SetBool("Moving", true); 
        
        return; // 아래 공격 로직 실행X
        }

        if (attackController.targetToAttack != null && move.isCommandedMove == false)
        {
            LookAtTarget();

            if (attackTimer <= 0)
            {
                Attack();
                Debug.Log("Player Attack");
                attackTimer = 1f / baseUnit.unitData.AttackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
            
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

            if (distanceFromTarget > baseUnit.unitData.AttackRange || attackController.targetToAttack == null)
            {
                animator.SetBool("Attack", false);
            }

        }
        else 
        {
            animator.SetBool("Attack", false);
        }
    }

    void Attack()
{
    //타겟이 사라졌는지 먼저 확인
    if (attackController.targetToAttack == null) return;
    
    float damageToInflict = baseUnit.unitData.Damage;

    //데미지를 넣을 수 있는 대상의 정보를 불러오기
    BaseUnit targetEntity = attackController.targetToAttack.GetComponent<BaseUnit>();

    //정말로 때릴 수 있는 상대인지 확인 후 데미지
    if (targetEntity != null)
    {
        targetEntity.TakeDamage(damageToInflict);
    }
}

    void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.rotation.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
