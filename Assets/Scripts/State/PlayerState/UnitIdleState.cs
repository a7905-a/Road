using UnityEngine;
using ProjectRoad.Unit;
using ProjectRoad.Controller;

namespace ProjectRoad.State
{
    public class UnitIdleState : StateMachineBehaviour
    {
        // 캐싱 컴포넌트
        private AttackController attackController;
        private BaseUnit baseUnit;
        private Move move;

        private bool isInitialized = false;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!isInitialized)
            {
                attackController = animator.GetComponent<AttackController>();
                baseUnit = animator.GetComponent<BaseUnit>();
                move = animator.GetComponent<Move>();
                
                isInitialized = true;
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (attackController.targetToAttack == null || move.isCommandedMove)
            {
                return;
            }

            //타겟과 거리 계산
            float distance = Vector3.Distance(animator.transform.position, attackController.targetToAttack.position);

            //사거기 안쪽이라면 공격
            if (distance <= baseUnit.CurrentAttackRange)
            {
                // 타겟을 볼려고 회전
                animator.transform.LookAt(attackController.targetToAttack);
                
                // 공격 애니메이션 조건 활성화
                animator.SetBool("Attack", true);
                
            }
            //사거리 밖이라면 추격
            else
            {
                if (move.isHolding) return;
                animator.SetBool("Follow", true);
            }
        }



        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

    }
}
