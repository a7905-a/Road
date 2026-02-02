using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public bool isCommandedMove;
    [SerializeField] LayerMask groud;

    Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groud))
            {
                isCommandedMove = true;
                agent.SetDestination(hit.point);
                animator.SetBool("Moving", true); 
            }
        }

        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedMove = false;
            animator.SetBool("Moving", false);
        }
        else
        {
            animator.SetBool("Moving", true);
        }
    }
}
