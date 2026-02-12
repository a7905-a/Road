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
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
            {
                isCommandedMove = false;
            }
        }
    }
}

