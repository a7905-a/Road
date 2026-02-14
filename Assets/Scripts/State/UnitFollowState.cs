using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;
    BaseUnit baseUnit;
    NavMeshAgent agent;
    Move move;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        baseUnit = animator.GetComponent<BaseUnit>();
        move = animator.GetComponent<Move>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack == null)
        {
            animator.SetBool("Follow", false);
        }
        else
        {
            if (move.isCommandedMove == false)
            {
                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

                if (distanceFromTarget <= baseUnit.unitData.AttackRange)
                {
                    //사거리 내부로 들어오면 공격 조건 활성화
                    animator.transform.LookAt(attackController.targetToAttack);
                    animator.SetBool("Attack", true);
                    
                }
                else
                {
                    //사거리 밖이라면 추격
                    agent.SetDestination(attackController.targetToAttack.position);
                    
                }
            }
        }

        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    
}
