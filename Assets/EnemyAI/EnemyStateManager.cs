using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public BaseUnit baseUnit;
    public IEnemyState CurrentState;
    public float attackTimer;
    public Animator animator;
    public NavMeshAgent agent;
    public SimplePatrol simplePatrol;
    public AttackController attackController;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        simplePatrol = GetComponent<SimplePatrol>();
        baseUnit = GetComponent<BaseUnit>();
        attackController = GetComponent<AttackController>();

    }

    void Start()
    {
        TransitionToState(new IdleState());
    }


    void Update()
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
