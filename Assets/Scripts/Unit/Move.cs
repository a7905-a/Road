using UnityEngine;
using UnityEngine.AI;

namespace ProjectRoad.Unit
{
    public class Move : MonoBehaviour
    {
        [SerializeField] LayerMask ground;
        [SerializeField] float crowdRadius = 3.0f;
        [SerializeField] float stuckTimeThreshold = 0.3f;
        float stuckTimer = 0f;

        Camera cam;
        NavMeshAgent agent;
        public bool isCommandedMove;
        public bool isHolding;

        

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            HandleMoveInput();
            CheckMove();
        }



        void CheckMove()
        {
            if (isCommandedMove)
            {
                CheckArrived();
            }
        }

        void HandleMoveInput()
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

                if (MoveToCursor())
                {
                    return;
                }

            }
        }

        bool MoveToCursor()
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                isCommandedMove = true;
                agent.SetDestination(hit.point);
                return true;
            }
            return false;
        }

        void CheckArrived()
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

            if (dist <= crowdRadius) 
            {
                // 속도가 거의 0에 가깝다면 (누군가에게 막혀서 못 가고 있다면)
                if (agent.velocity.sqrMagnitude < 0.1f) 
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
            else
            {
                // 목표 지점 근처도 아니라면 타이머 초기화 (지나가다 맵 지형에 걸린 건 무시)
                stuckTimer = 0f; 
            }
        }
        void StopMovement()
        {
            isCommandedMove = false;
            stuckTimer = 0f;
            agent.ResetPath();
        }
    }
}

