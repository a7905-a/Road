using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    AttackController attackController;
    BaseUnit baseUnit;
    Move move;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        baseUnit = animator.GetComponent<BaseUnit>();
        move = animator.GetComponent<Move>();
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
        if (distance <= baseUnit.unitData.AttackRange)
        {
            // 타겟을 볼려고 회전
            animator.transform.LookAt(attackController.targetToAttack);
            
            // 공격 애니메이션 조건 활성화
            animator.SetBool("Attack", true);
        }
        //사거리 밖이라면 추격
        else
        {
            animator.SetBool("Follow", true);
        }
    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
