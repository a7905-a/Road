using System.Collections.Generic;
using UnityEngine;

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
}
