using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    AttackController attackController;
    Unit unit;
    Move move;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        unit = animator.GetComponent<Unit>();
        move = animator.GetComponent<Move>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack == null || move.isCommandedMove)
        {
            return;
        }

        // 2. 타겟이 있다면 거리를 재본다.
        float distance = Vector3.Distance(animator.transform.position, attackController.targetToAttack.position);

        // 3. 사거리 안이라면? -> 다시 공격 시작!
        if (distance <= unit.unitData.AttackRange)
        {
            // 적을 바라보게 하고 (선택 사항)
            animator.transform.LookAt(attackController.targetToAttack);
            
            // 공격 애니메이션 발동! -> AttackState로 전이됨
            animator.SetBool("Attack", true);
        }
        // 4. 사거리 밖이라면? -> 추격 시작!
        else
        {
            animator.SetBool("Follow", true);
        }
    }
        // if (attackController.targetToAttack != null)
        // {
        //     animator.SetBool("Follow", true);
        // }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
