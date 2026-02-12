using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;

    NavMeshAgent agent;

    public float attackDistance = 1f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        attackController.SetFollowMaterial();
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
            if (animator.transform.GetComponent<Move>().isCommandedMove == false)
            {
                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
                if (distanceFromTarget <= attackDistance)
                {
                    
                    animator.transform.LookAt(attackController.targetToAttack); // 휙 돌아버리까 Slerp로 보안
                    animator.SetBool("Attack", true);
                    
                }
                else
                {
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
