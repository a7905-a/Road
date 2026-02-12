using UnityEngine;

public enum AttackTargetType
{
    Player,
    Enemy
}

public class AttackController : MonoBehaviour
{
    [SerializeField] AttackTargetType targetType;
    public Transform targetToAttack;

    public int  unitDamage; //so로 변경 예정
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


    public void SetIdleMaterial()
    {
        //GetComponent<Renderer>().material = idleStateMaterial;
    }

    public void SetFollowMaterial()
    {
        //GetComponent<Renderer>().material = followStateMaterial;
    }

    public void SetAttackMaterial()
    {
       //GetComponent<Renderer>().material = attackStateMaterial;
    }


    
}
