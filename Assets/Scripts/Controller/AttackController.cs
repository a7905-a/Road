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
        [SerializeField] AttackTargetType targetType;
        public Transform targetToAttack;

        string cachedTargetTag;
        void Awake()
        {
            cachedTargetTag = targetType.ToString();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(cachedTargetTag) && targetToAttack == null)
            {
                targetToAttack = other.transform;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(cachedTargetTag) && targetToAttack == null)
            {
                targetToAttack = other.transform;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(cachedTargetTag) && targetToAttack != null)
            {
                targetToAttack = null;
            }
        }
    }
}
