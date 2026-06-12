using UnityEngine;
using ProjectRoad.Unit;

namespace ProjectRoad.Controller
{
    public enum AttackTargetType
    {
        Player,
        Enemy
    }

    public class AttackController : MonoBehaviour, IAction
    {
        [SerializeField] private AttackTargetType targetType;
        public Transform targetToAttack;

        private string cachedTargetTag;
        private ActionScheduler actionScheduler;
        private Animator animator;

        private void Awake()
        {
            cachedTargetTag = targetType.ToString();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(cachedTargetTag) && targetToAttack == null)
            {
                targetToAttack = other.transform;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(cachedTargetTag) && targetToAttack == null)
            {
                targetToAttack = other.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(cachedTargetTag) && targetToAttack != null)
            {
                targetToAttack = null;
            }
        }

        public void StartAttackAction(Transform target)
        {
            if (target == null) return;

            actionScheduler.StartAction(this);
            targetToAttack = target;

            animator.SetBool("Attack", true);
            animator.SetBool("Moving", false);
        }

        public void Cancel()
        {
            targetToAttack = null;
            animator.SetBool("Attack", false);
        }
    }
}
