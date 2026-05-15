using UnityEngine;
using UnityEngine.AI;

namespace ProjectRoad.Unit
{
    public class BaseUnit : MonoBehaviour
    {
        [Header("같이 받는 데이터")]
        public UnitData unitData;
        [SerializeField] protected int myUnitID;
        [SerializeField] protected HealthTracker healthTracker;

        //protected float currentHealth;
        protected NavMeshAgent agent;
        protected Animator animator;

        [SerializeField] protected float currentHealth;
        [SerializeField] protected float currentDamage;
        [SerializeField] protected float currentMoveSpeed;

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
                currentHealth = myData.MaxHealth;
                currentDamage = myData.Damage;
                currentMoveSpeed = myData.MoveSpeed;
                if (agent != null)
                {
                    agent.speed = currentMoveSpeed;
                }
            }
            else
            {
                Debug.LogError("유닛 데이터가 없습니다.");
            }

            

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
            OnHit();    

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

        protected virtual void OnHit()
        { }
    }
}
