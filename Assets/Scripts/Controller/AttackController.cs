using UnityEngine;

namespace ProjectRoad.Controller
{
    public enum AttackTargetType
    {
        Player,
        Enemy
    }

    public class AttackController : MonoBehaviour
    {
        [SerializeField] private AttackTargetType targetType;
        public Transform targetToAttack;

        private string cachedTargetTag;
        private void Awake()
        {
            cachedTargetTag = targetType.ToString();
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
    }
}
