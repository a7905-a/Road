using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public IEnemyState CurrentState;

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
