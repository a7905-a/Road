using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    [SerializeField] LayerMask groud;
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
            MoveToCursor();
        }

        //길이 없거나 혹은 도착했을 때 이동 명령을 끝내기 위한 조건문
        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedMove = false;

        }

    }

    void MoveToCursor()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groud))
        {
            isCommandedMove = true;
            agent.SetDestination(hit.point);
        }
    }
}
