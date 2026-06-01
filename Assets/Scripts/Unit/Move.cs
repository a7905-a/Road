using UnityEngine;
using UnityEngine.AI;

namespace ProjectRoad.Unit
{
    public class Move : MonoBehaviour
    {
        [Header("이동 가능 레이어")]
        [SerializeField] private LayerMask ground;

        [Header("병목 이동 설정")]
        [SerializeField] private float crowdRadius = 3.0f;
        [SerializeField] private float minVelocityToMove = 2.5f ;
        [SerializeField] private float stuckTimeThreshold = 1.0f;
        private float stuckTimer = 0f;


        // 유닛의 현재 이동 상태
        public bool isCommandedMove;
        public bool isHolding;

        // 캐싱 컴포넌트
        private Camera cam;
        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            CheckMove();
            HandleMoveInput();
        }

        private void CheckMove()
        {
            if (isCommandedMove)
            {
                CheckArrived();
            }
        }

        private void HandleMoveInput()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                isHolding = !isHolding;
                agent.SetDestination(transform.position);
                isCommandedMove = false;

            }

            if (Input.GetMouseButtonDown(1))
            {
                if (isHolding)
                {
                    isHolding = false;
                }

                // if (MoveToCursor())
                // {
                //     return;
                // }

            }
        }

        private bool MoveToCursor()
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                MoveToPosition(hit.point);
                agent.avoidancePriority = 50;
                return true;
            }
            return false;
        }

        public void MoveToPosition(Vector3 position)
        {
            isCommandedMove = true;
            agent.SetDestination(position);
        }
        private void CheckArrived()
        {   
            //경로를 계산중이라면 true를 반환함 그래서 경로 계산이 다 끝난 상태에서 거리 계산을 해야 되서 return을 넣었다
            if (agent.pathPending) return;
            
            float dist = agent.remainingDistance;

            if (dist <= agent.stoppingDistance)
            {
                //sqrMagnitude는 제곱합만 가져옴
                //유닛의 속도가 0인가를 확인할 때 속도가 0이라면 속도의 제곱도 0이다. 결과는 똑같은데 sqrMagnitude는 루트 계산을 생략할 수 있어서 성능최적화에서 이득
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
                {   
                    StopMovement();
                    return;
                }
            }

            if (isCommandedMove) 
            {
                //누군가에게 막혀서 못 가고 있다면
                if (agent.velocity.sqrMagnitude < minVelocityToMove) 
                {
                    stuckTimer += Time.deltaTime; // 타이머 증가

                    // 막힌 지 0.5초(stuckTimeThreshold)가 넘었다면? 
                    if (stuckTimer >= stuckTimeThreshold)
                    {
                        StopMovement(); // "나도 도착한 걸로 칠게!" 하고 멈춤
                    }
                }
                else
                {
                    // 다시 틈이 생겨서 이동하기 시작하면 타이머 초기화
                    stuckTimer = 0f; 
                }
            }
            
        }
        private void StopMovement()
        {
            isCommandedMove = false;
            stuckTimer = 0f;
            agent.ResetPath();
            agent.avoidancePriority = 0;
        }
    }
}

