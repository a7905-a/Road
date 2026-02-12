using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    Unit unit;



    public float attackRate = 2f;
    float attackTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        unit = animator.GetComponent<Unit>();
        attackController.SetAttackMaterial();

        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Move>().isCommandedMove == true)
        {
        //공격 상태 즉시 해제
        animator.SetBool("Attack", false);
        
        //이동 상태 켜주기
        animator.SetBool("Moving", true); 
        
        return; // 아래 공격 로직 실행하지 말고 바로 리턴
        }

        if (attackController.targetToAttack != null && animator.transform.GetComponent<Move>().isCommandedMove == false)
        {
            LookAtTarget();

            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = 1f / attackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
            
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

            if (distanceFromTarget > unit.unitData.AttackRange || attackController.targetToAttack == null)
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
    // 1. 방어 코드: 타겟이 사라졌는지 먼저 확인
    if (attackController.targetToAttack == null) return;
    
    float damageToInflict = unit.unitData.Damage; // 오타 수정 (Io -> To)

    // 2. 핵심 수정: <Unit> 대신 부모 클래스인 <BaseUnit> (또는 LivingEntity) 사용
    // 이렇게 해야 Unit(플레이어)이든 Enemy(적)든 가리지 않고 "체력 있는 놈"은 다 때립니다.
    BaseUnit targetEntity = attackController.targetToAttack.GetComponent<BaseUnit>();

    // 3. 안전 장치: 정말로 때릴 수 있는 상대인지 확인 후 데미지
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
