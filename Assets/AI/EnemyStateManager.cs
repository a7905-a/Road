using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public IEnemyState CurrentState;
    public Transform currentTarget;
    public Animator animator;
    public NavMeshAgent agent;
    public SimplePatrol simplePatrol;
    public EnemyDateManger enemyDataManager;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        simplePatrol = GetComponent<SimplePatrol>();
        enemyDataManager = GetComponent<EnemyDateManger>();

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
        // CurrentState?는 CurrentState가 null이 아닐 때만 실행되도록 하는 null 조건부 연산자
        CurrentState?.ExitState(this);
        CurrentState = newState;
        CurrentState.EnterState(this);
        
    }
}
