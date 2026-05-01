using System.Collections.Generic;
using UnityEngine;
using ProjectRoad.Unit;
using ProjectRoad.Controller;
using System.Linq;

namespace ProjectRoad.Manager
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public static UnitSelectionManager Instance { get; private set; }

        //메모리 효율성을 더 올릴려면 new List<GameObject>(100) 처럼 이렇게 미리 크기를 지정해주는게 좋다.
        //리스트에 유닛이 추가, 없어지게 하는건 메서드 만을 사용해야 하기 때문에 읽기 전용으로 설정
        [SerializeField] List<GameObject> allUnitsList = new List<GameObject>();
        public IReadOnlyList<GameObject> AllUnitList => allUnitsList;

        [SerializeField] List<GameObject> unitsSelected = new List<GameObject>();
        public IReadOnlyList<GameObject> SelectedUnits => unitsSelected;
        
        [SerializeField] LayerMask clickable;
        [SerializeField] LayerMask ground;
        [SerializeField] LayerMask attackable;
        [SerializeField] GameObject groundMarker; 
        [SerializeField] bool attackCursorVisible;
        [SerializeField] float formationSpacing = 2.0f;
        Camera cam;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            cam = Camera.main;
        }   

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                
                //클릭 가능한 오브젝트를 체크
                //Shift키 누르면서 체크하면 다중 체크
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MultSelect(hit.collider.gameObject);
                    }
                    else
                    {
                        SelectByClick(hit.collider.gameObject);
                    }
                }
                //클릭 가능하지 않은 오브젝트를 체크
                //땅 클릭 시 유닛 리스트를 비우기
                else
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        
                        ClearSelection();
                    }
                    
                }
            }


            if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                
                //클릭 가능한 오브젝트를 체크
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    groundMarker.transform.position = hit.point;
                    groundMarker.SetActive(false);
                    groundMarker.SetActive(true);

                    List<Vector3> formationPositions = SetBFSPositions(hit.point, unitsSelected.Count, formationSpacing);
                     // 목적지에 가까운 유닛 순으로 오름차순 정렬
                    var sortedUnits = unitsSelected.OrderBy(u => Vector3.Distance(hit.point, u.transform.position)).ToList();
                    // 목적지에서 멀리 있는 자리 순으로 내림차순 정렬
                    var sortedPositions = formationPositions.OrderByDescending(p => Vector3.Distance(hit.point, p)).ToList();

                    for (int i = 0; i < sortedUnits.Count; i++)
                    {
                        // 자리가 부족할 수 있으므로 방어막
                        if (i < sortedPositions.Count) 
                        {
                            Move moveScript = sortedUnits[i].GetComponent<Move>();
                            if (moveScript != null)
                            {
                                moveScript.MoveToPosition(sortedPositions[i]);
                            }
                        }
                    }



                }
            }

            // 공격 대상
            if (unitsSelected.Count > 0)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                
                //클릭 가능한 오브젝트를 체크
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
                {
                    Debug.Log("Enemy Clicked");

                    attackCursorVisible = true;

                    if (Input.GetMouseButtonDown(1))
                    {
                        Transform target = hit.transform;
                        foreach (GameObject unit in unitsSelected)
                        {
                            if (unit.GetComponent<AttackController>())
                            {
                                unit.GetComponent<AttackController>().targetToAttack = target;
                            }
                        }
                    }
                }
                else
                {
                    attackCursorVisible = false;
                }
            }

        }

        public void ClearSelection()
        {
            foreach (GameObject unit in unitsSelected)
            {
                SelectUnit(unit, false);
            }
            
            groundMarker.SetActive(false);

            unitsSelected.Clear();
        }

        public void AddUnit(GameObject gameObject)
        {
            if (!allUnitsList.Contains(gameObject))
            {
                allUnitsList.Add(gameObject);
            }
        }
        public void RemoveUnit(GameObject gameObject)
        {
            if (allUnitsList.Contains(gameObject))
            {
                allUnitsList.Remove(gameObject);
            }

            if (unitsSelected.Contains(gameObject))
            {
                unitsSelected.Remove(gameObject);
            }
        }


        void MultSelect(GameObject unit)
        {
            if (unitsSelected.Contains(unit) == false)
            {
                unitsSelected.Add(unit);
                SelectUnit(unit, true);
            }
            else
            {
                SelectUnit(unit, false);
                unitsSelected.Remove(unit);
            }
        }

        

        void SelectByClick(GameObject unit)
        {
            ClearSelection();

            unitsSelected.Add(unit);

            SelectUnit(unit, true);
        }

        void EnableUnitMovement(GameObject unit, bool moveTrigger)
        {
            unit.GetComponent<Move>().enabled = moveTrigger;
        }

        void TriggerSelectionIndicator(GameObject unit, bool isVisible)
        {
            unit.transform.GetChild(0).gameObject.SetActive(isVisible);
        }

        void SelectUnit(GameObject unit, bool isSelected)
        {
            EnableUnitMovement(unit, isSelected);
            TriggerSelectionIndicator(unit, isSelected);
        }

        internal void DragSelect(GameObject unit)
        {
            if (unitsSelected.Contains(unit) == false)
            {
                unitsSelected.Add(unit);
                TriggerSelectionIndicator(unit, true);
                EnableUnitMovement(unit, true);
            }
        }

        //center는 이동 기능에 사용할 레이캐스트의 hit.point, requiredCount는 선택된 유닛의 수, spacing은 유닛 간의 간격
        List<Vector3> SetBFSPositions(Vector3 center, int requiredCount, float spacing)
        {
            // 최종 목적지를 담을 리스트와 BFS를 위한 큐, 방문한 위치를 담을 해시셋
            List<Vector3> validPos = new List<Vector3>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            Vector2Int startPos = Vector2Int.zero; // BFS 시작점 (0,0)
            queue.Enqueue(startPos);
            visited.Add(startPos);

            Vector2Int[] directions = new Vector2Int[] // BFS 탐색을 위한 4방향 벡터
            {
                new Vector2Int(1, 0),   // 오른쪽
                new Vector2Int(-1, 0),  // 왼쪽
                new Vector2Int(0, 1),   // 위
                new Vector2Int(0, -1)   // 아래
            };

            while (queue.Count > 0 && validPos.Count < requiredCount)
            {
                Vector2Int current = queue.Dequeue();
                Vector3 worldPos = center + new Vector3(current.x, 0, current.y) * spacing;

                if(IsValidNavMeshPosition(worldPos, out Vector3 validPoint, spacing))
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
        bool IsValidNavMeshPosition(Vector3 samplePoint, out Vector3 resultPoint, float maxDistance)
        {
            UnityEngine.AI.NavMeshHit hit;
            // 주어진 점 주변에서 NavMesh 위를 찾음
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
