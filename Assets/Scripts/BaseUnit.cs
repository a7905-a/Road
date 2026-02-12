using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : MonoBehaviour
{
    [Header("같이 받는 데이터")]
    public UnitData unitData;
    [SerializeField] protected HealthTracker healthTracker;

    protected float currentHealth;
    protected NavMeshAgent agent;
    protected Animator animator;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (unitData != null)
        {
            currentHealth = unitData.MaxHealth;
            if (healthTracker != null)
            {
                healthTracker.UpdateSliderValue(currentHealth, unitData.MaxHealth);
            }
        }
    }
    // 데미지 공통 메서드
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (healthTracker != null && unitData != null)
            healthTracker.UpdateSliderValue(currentHealth, unitData.MaxHealth);

        if (currentHealth <= 0)
        {
            Retire();
        }
    }
    protected virtual void Retire()
    {
        Destroy(gameObject);
    }
}
