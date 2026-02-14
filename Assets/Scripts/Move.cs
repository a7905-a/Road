using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    Camera cam;
    NavMeshAgent agent;
    public bool isCommandedMove;

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
        if (Input.GetMouseButtonDown(1))
        {
            if (MoveToCursor())
            {
                return;
            }
            
        }

        if (isCommandedMove)
        {
            CheckArrived();
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

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //sqrMagnitude는 제곱합만 가져옴
            //유닛의 속도가 0인가를 확인할 때 속도가 0이라면 속도의 제곱도 0이다. 결과는 똑같은데 sqrMagnitude는 루트 계산을 생략할 수 있어서 성능최적화에서 이점을 가짐
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
            {
                isCommandedMove = false;
            }
        }
    }
}

