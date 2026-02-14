using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IEnemyState
{

    //float attackTimer = 0;

    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("Attack");
    }

    public void ExitState(EnemyStateManager enemy)
    {
        
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.attackController.targetToAttack == null)
        {
            if (Random.value < 0.5f)
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
            LookAtPlayer(enemy);
            enemy.attackTimer -= Time.deltaTime;
            if (enemy.attackTimer <= 0)
            {
                Attack(enemy);
                Debug.Log("Enemy Attack");
                enemy.attackTimer = 1f / enemy.baseUnit.unitData.AttackRate;
            }

            float distanceFromTarget = Vector3.Distance(enemy.attackController.targetToAttack.position, enemy.transform.position);

            if (distanceFromTarget > enemy.baseUnit.unitData.AttackRange)
            {
                enemy.TransitionToState(new ChaseState());
            }
        }
    }

    void LookAtPlayer(EnemyStateManager enemy)
    {
        Vector3 direction = enemy.attackController.targetToAttack.position - enemy.transform.position;
        enemy.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = enemy.transform.rotation.eulerAngles.y;
        enemy.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    void Attack(EnemyStateManager enemy)
{
    // 1. 방어 코드: 타겟이 사라졌는지 먼저 확인
    if (enemy.attackController.targetToAttack == null) return;
    
    float damageToInflict = enemy.baseUnit.unitData.Damage; // 오타 수정 (Io -> To)

    // 2. 핵심 수정: <Unit> 대신 부모 클래스인 <BaseUnit> (또는 LivingEntity) 사용
    // 이렇게 해야 Unit(플레이어)이든 Enemy(적)든 가리지 않고 "체력 있는 놈"은 다 때립니다.
    BaseUnit targetEntity = enemy.attackController.targetToAttack.GetComponent<BaseUnit>();

    // 3. 안전 장치: 정말로 때릴 수 있는 상대인지 확인 후 데미지
    if (targetEntity != null)
    {
        targetEntity.TakeDamage(damageToInflict);
    }
}
}
