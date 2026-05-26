using Unity.VisualScripting;
using UnityEngine;
using ProjectRoad.Unit;

namespace ProjectRoad.State
{
    public class AttackState : IEnemyState
    {
        static readonly int AttackHash = Animator.StringToHash("Attack");

        public void EnterState(EnemyStateManager enemy)
        {
            enemy.animator.Play(AttackHash);
            enemy.agent.ResetPath();
            enemy.agent.velocity = Vector3.zero;
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
                    

                    enemy.attackTimer = 1f / enemy.baseUnit.CurrentAttackRate;
                }

                float distanceFromTarget = Vector3.Distance(enemy.attackController.targetToAttack.position, enemy.transform.position);

                if (distanceFromTarget > enemy.baseUnit.CurrentAttackRange)
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
        // 타겟이 사라졌는지 확인
        if (enemy.attackController.targetToAttack == null) return;
        
        float damageToInflict = enemy.baseUnit.CurrentDamage;

        BaseUnit targetEntity = enemy.attackController.targetToAttack.GetComponent<BaseUnit>();

        // 정말로 때릴 수 있는 상대인지 확인 후 데미지
        if (targetEntity != null)
        {
            targetEntity.TakeDamage(damageToInflict);
        }
        }
    }
}
