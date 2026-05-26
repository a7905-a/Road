using System.Collections.Generic;
using System.Linq;
using ProjectRoad.Controller;
using ProjectRoad.Unit;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectRoad.Manager
{
    public class UnitCommandManager : MonoBehaviour
    {
        public static UnitCommandManager Instance;

        [Header("레이어 설정")]
        [SerializeField] private LayerMask ground;
        [SerializeField] private LayerMask attackable;

        [Header("부대 이동 설정")]
        [SerializeField] private float formationSpacing = 2.0f;

        [Header("시각효과 UI")]
        [SerializeField] private GameObject groundMarker;
        [SerializeField] private bool attackCursorVisible;

        private Camera cam;



        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            HandleCommands();
        }

        private void HandleCommands()
        {
            var selectedUnits = UnitSelectionManager.Instance.SelectedUnits;
            if (selectedUnits.Count == 0) return;

            if (Input.GetMouseButtonDown(1)) // 마우스 우클릭 감지
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                // 적을 클릭했는지 확인
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
                {
                    CommandAttack(selectedUnits, hit.transform);
                    return;
                }

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    CommandMove(selectedUnits, hit.point);
                }
            }
        }

        private void CommandAttack(IReadOnlyList<GameObject> units, Transform target)
        {
            Debug.Log("적 클릭");
            attackCursorVisible = true;

            foreach(GameObject unit in units)
            {
                AttackController attackController = unit.GetComponent<AttackController>();
                attackController.targetToAttack = target;
            }   
        }

        private void CommandMove(IReadOnlyList<GameObject> units, Vector3 destination)
        {
            attackCursorVisible = false;

            groundMarker.transform.position = destination;
            groundMarker.SetActive(false);
            groundMarker.SetActive(true);

            List<Vector3> formationPosition = SetBFSPosition(destination, units.Count, formationSpacing);

            var sortedUnits = units.OrderBy(u => Vector3.Distance(destination, u.transform.position)).ToList();
            var sortedPositions = formationPosition.OrderByDescending(p => Vector3.Distance(destination, p)).ToList();

            for (int i = 0; i < sortedUnits.Count; i++)
            {
                if (i < sortedPositions.Count)
                {
                    Move moveScript = sortedUnits[i].GetComponent<Move>();
                    moveScript.MoveToPosition(sortedPositions[i]);
                }
            }
        }

        private List<Vector3> SetBFSPosition(Vector3 center, int requiredCount, float spacing)
        {
            List<Vector3> validPos = new List<Vector3>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            Vector2Int startPos = Vector2Int.zero;
            queue.Enqueue(startPos);
            visited.Add(startPos);

            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(1, 0), // 오른쪽
                new Vector2Int(-1, 0), // 왼쪽
                new Vector2Int(0, 1), // 위
                new Vector2Int(0, -1) // 아래
            };

            while(queue.Count > 0 && validPos.Count < requiredCount)
            {
                Vector2Int current = queue.Dequeue();
                Vector3 worldPos = center + new Vector3(current.x, 0, current.y) * spacing;

                if (IsValidNavMeshPosition(worldPos, out Vector3 validPoint, spacing))
                {
                    validPos.Add(validPoint);
                }
                
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int neighborGrid = current + dir;
                    if (!visited.Contains(neighborGrid))
                    {
                        visited.Add(neighborGrid);
                        queue.Enqueue(neighborGrid);
                    }
                }
            }
                return validPos;
        }

        private bool IsValidNavMeshPosition(Vector3 samplePoint, out Vector3 resultPoint, float maxDistance)
        {
            UnityEngine.AI.NavMeshHit hit;

            if (UnityEngine.AI.NavMesh.SamplePosition(samplePoint, out hit, maxDistance / 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                resultPoint = hit.position;
                return true;
            }
            resultPoint = Vector3.zero;
            return false;
        }
    }
}

