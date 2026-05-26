using UnityEngine;
using UnityEngine.AI;

namespace ProjectRoad.Unit
{
    public class BaseUnit : MonoBehaviour
    {
        [Header("유닛 식별 데이터")]
        [SerializeField] protected int myUnitID;

        [Header("컴포넌트 참조")]
        [SerializeField] protected HealthTracker healthTracker;

        [Header("유닛 스탯 참고용")]
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float currentDamage;
        [SerializeField] protected float currentMoveSpeed;
        [SerializeField] protected float currentAttackRate;
        [SerializeField] protected float currentAttackRange;

        public float CurrentHealth
        {
            get { return currentHealth; }
        }

        public float CurrentDamage
        {
            get { return currentDamage; }
        }

        public float CurrentMoveSpeed
        {
            get { return currentMoveSpeed; }
        }
        public float CurrentAttackRate
        {
            get { return currentAttackRate; }
        }
        public float CurrentAttackRange
        {
            get { return currentAttackRange; }
        }

        
        protected NavMeshAgent agent;
        protected Animator animator;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            RoadUnitData myData = DataManager.Instance.GetUnitDataByID(myUnitID);

            if (myData != null)
            {
                maxHealth = myData.MaxHealth;
                currentHealth = myData.MaxHealth;
                currentDamage = myData.Damage;
                currentMoveSpeed = myData.MoveSpeed;
                currentAttackRate = myData.AttackRate;
                currentAttackRange = myData.AttackRange;

                if (agent != null)
                {
                    agent.speed = currentMoveSpeed;
                }

                if (healthTracker != null)
                {
                    healthTracker.UpdateSliderValue(currentHealth, maxHealth);
                }
            }
            else
            {
                Debug.LogError("유닛 데이터가 없습니다.");
            }
        }

        // 데미지 공통 메서드
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            OnHit();    

            if (healthTracker != null)
            {
                healthTracker.UpdateSliderValue(currentHealth, maxHealth);
            }
                
            if (currentHealth <= 0)
            {
                Retire();
            }
        }
        protected virtual void Retire()
        {
            Destroy(gameObject);
        }

        protected virtual void OnHit()
        {
            // 자식 클래스에서 오버라이드하여 피격 로직(이펙트, 사운드 등) 구현
        }
    }
}
