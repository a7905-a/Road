using System.Collections.Generic;
using UnityEngine;
using ProjectRoad.Unit;
using ProjectRoad.Controller;
using System.Linq;

namespace ProjectRoad.Manager
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public static UnitSelectionManager Instance;

        [Header("유닛 상태")]
        //메모리 효율성을 더 올리고 싶으면 new List<GameObject>(100) 처럼 이렇게 미리 크기를 지정해주는게 좋다.
        //리스트에 유닛이 추가, 없어지게 하는건 메서드 만을 사용해야 하기 때문에 읽기 전용으로 설정
        [SerializeField] private List<GameObject> allUnitsList = new List<GameObject>(100);
        public IReadOnlyList<GameObject> AllUnitList => allUnitsList;

        [SerializeField] private List<GameObject> unitsSelected = new List<GameObject>(50);
        public IReadOnlyList<GameObject> SelectedUnits => unitsSelected;

        
        [Header("선택 레이어 설정")]
        [SerializeField] private LayerMask clickable;

        // 캐싱 컴포넌트
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

        }

        public void ClearSelection()
        {
            foreach (GameObject unit in unitsSelected)
            {
                SelectUnit(unit, false);
            }
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


        private void MultSelect(GameObject unit)
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

        

        private void SelectByClick(GameObject unit)
        {
            ClearSelection();

            unitsSelected.Add(unit);

            SelectUnit(unit, true);
        }

        private void EnableUnitMovement(GameObject unit, bool moveTrigger)
        {
            unit.GetComponent<Move>().enabled = moveTrigger;
        }

        private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
        {
            unit.transform.GetChild(0).gameObject.SetActive(isVisible);
        }

        private void SelectUnit(GameObject unit, bool isSelected)
        {
            //EnableUnitMovement(unit, isSelected);
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

    }
}
