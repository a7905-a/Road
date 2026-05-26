using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRoad.Manager
{
    public class UnitSelectionBox : MonoBehaviour
    {
        [Header("드레그 박스 이미지")]
        [SerializeField] private RectTransform boxVisual;
        private Rect selectionBox;
        private Vector2 startPosition;
        private Vector2 endPosition;

        // 캐싱 컴포넌트
        private Camera myCamera;

        private void Start()
        {
            myCamera = Camera.main;
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }

        private void Update()
        {
            // 마우스 왼쪽 클릭 확인
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;

                // 박스 모양 초기화
                selectionBox = new Rect();
            }

            
            if (Input.GetMouseButton(0))
            {
                if (boxVisual.rect.width > 0 || boxVisual.rect.height > 0)
                {
                    UnitSelectionManager.Instance.ClearSelection();
                    
                    SelectUnits();
                }

                endPosition = Input.mousePosition;
                DrawVisual();
                DrawSelection();
            }

            // 마우스에서 손 떼는 순간
            if (Input.GetMouseButtonUp(0))
            {
                SelectUnits();

                startPosition = Vector2.zero;
                endPosition = Vector2.zero;
                DrawVisual();
            }
        }

        private void DrawVisual()
        {

            Vector2 boxStart = startPosition;
            Vector2 boxEnd = endPosition;

            // 상자의 정중앙을 위치로 잡기 위한 식
            Vector2 boxCenter = (boxStart + boxEnd) / 2;
            boxVisual.position = boxCenter;

            // 어느 방향으로 드래그 하든 무조건 양수가 나와야 하니 절대값을 씌워줌
            Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

            // 구한 값을 UI 인스펙터 창의 Width와 Height에 대입
            boxVisual.sizeDelta = boxSize;
        }

        private void DrawSelection()
        {
            if (Input.mousePosition.x < startPosition.x)
            {
                selectionBox.xMin = Input.mousePosition.x;
                selectionBox.xMax = startPosition.x;
            }
            else
            {
                selectionBox.xMin = startPosition.x;
                selectionBox.xMax = Input.mousePosition.x;
            }

            if (Input.mousePosition.y < startPosition.y)
            {
                selectionBox.yMin = Input.mousePosition.y;
                selectionBox.yMax = startPosition.y;
            }
            else
            {
                selectionBox.yMin = startPosition.y;
                selectionBox.yMax = Input.mousePosition.y;
            }
        }

        private void SelectUnits()
        {
            foreach (var unit in UnitSelectionManager.Instance.AllUnitList)
            {
                if (unit.CompareTag("Enemy")) continue;
                
                if (selectionBox.Contains(myCamera.WorldToScreenPoint(unit.transform.position)))
                {
                    UnitSelectionManager.Instance.DragSelect(unit);
                }
            }
        }
    }
}
