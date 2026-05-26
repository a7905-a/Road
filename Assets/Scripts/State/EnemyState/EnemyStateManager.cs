using UnityEngine;
using UnityEngine.AI;
using ProjectRoad.Unit;
using ProjectRoad.Controller;

namespace ProjectRoad.State
{
    public class EnemyStateManager : MonoBehaviour
    {
        // 상태 변수
        public float attackTimer;

        // 캐싱 컴포넌트
        public Animator animator;
        public NavMeshAgent agent;
        public SimplePatrol simplePatrol;
        public BaseUnit baseUnit;
        public AttackController attackController;

        // 인터페이스로 정의한 상태
        public IEnemyState CurrentState;
        

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            simplePatrol = GetComponent<SimplePatrol>();
            baseUnit = GetComponent<BaseUnit>();
            attackController = GetComponent<AttackController>();

        }

        private void Start()
        {
            TransitionToState(new IdleState());
        }


        private void Update()
        {
            CurrentState?.UpdateState(this);
        }

        public void TransitionToState(IEnemyState newState)
        {
            Debug.Log($"상태 전환: {CurrentState?.GetType().Name} -> {newState.GetType().Name}");
            // CurrentState?는 CurrentState가 null이 아닐 때만 실행되도록 하는 null 조건부 연산자
            CurrentState?.ExitState(this);
            CurrentState = newState;
            CurrentState.EnterState(this);
            
        }
    }
}
