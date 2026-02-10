using UnityEngine;

public class PatrolState : IEnemyState
{
    float moveTime = 0;
    float moveTimer = 0;
    bool isPatrolling = false;
    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("Walk");
        isPatrolling = false;
        moveTimer = 0f;
        moveTime = Random.Range(2f, 5f);
        
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Patrol State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        // if (enemy.GetComponent<EnemySight>().IsPlayerInSight())
        // {
        //     enemy.TransitionToState(new ChaseState());
        //     return;
        // }



        // 1. 회전 처리 (강의 내용 유지)
    if (!isPatrolling)
    {
        // 아까 수정한 3D 회전 함수 호출 (뒤로 홱 돕니다)
        //enemy.GetComponent<EnemyFlip>().Flip(); 
        isPatrolling = true;
    }

    // 2. 이동 방향 결정 (핵심 변경 부분!)
    // 2D: 스케일 체크 (X) -> 3D: 현재 오브젝트의 앞쪽 방향(Forward)을 가져옴 (O)
    Vector3 dir = enemy.transform.forward; 

    moveTimer += Time.deltaTime;

    // 3. 실제 이동 적용 (Rigidbody)
    // 3D니까 Vector2 대신 Vector3를 사용해야 합니다.
    // linearVelocity는 Unity 6(Preview) API이거나 사용자 정의 변수 같습니다. 
    // 만약 일반 Unity 버전이라면 rb.velocity를 쓰시면 됩니다.
    
    // Y축 속도(낙하)는 유지하고, X/Z 축으로만 이동하게 설정


    //float currentYVelocity = enemy.rb.velocity.y; // 기존 낙하 속도 유지
    
    // dir(앞쪽) * 속도


    //enemy.rb.velocity = dir * enemy.GetComponent<EnemyDataManager>()._enemyData.PatrolSpeed;
    
    // 만약 Y축(중력)을 살려야 한다면 아래처럼 Y값을 덮어씌우지 않게 주의하세요.
    // enemy._rb.velocity = new Vector3(enemy._rb.velocity.x, currentYVelocity, enemy._rb.velocity.z);


    // 4. 시간 종료 체크 (강의 내용 유지)
    if (moveTimer >= moveTime)
    {
        //enemy.rb.velocity = Vector3.zero; // 멈춤
        enemy.TransitionToState(new IdleState());
    }
    }
}
