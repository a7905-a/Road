using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectRoad.Controller
{
    public class RTSCameraController : MonoBehaviour
    {
        // 싱글톤
        public static RTSCameraController instance;

        [Header("카메라 이동 범위 설정")]
        [SerializeField] BoxCollider cameraConfiner;

        [Header("시작 카메라 위치 설정")] 
        [SerializeField] Transform startCameraTransform;
        [SerializeField] Transform cameraTransform;
        [SerializeField] Transform followTransform; //프로퍼티로 변경 가능
        
        Vector3 newPosition;

        [Header("화면 움직임 설정")]
        [SerializeField] bool moveWithKeyboad;
        [SerializeField] bool moveWithEdgeScrolling;
        [SerializeField] bool moveWithMouseDrag;


        [Header("전체적인 움직임 속도 설정")]
        [SerializeField] float fastSpeed = 0.05f;
        [SerializeField] float normalSpeed = 0.01f;
        [SerializeField] float movementSensitivity = 1f;
        float movementSpeed;

        [Header("엣지 스크롤 설정")]
        [SerializeField] float edgeSize = 50f;

        bool isCursorSet = false;
        public Texture2D cursorArrowUp;
        public Texture2D cursorArrowDown;
        public Texture2D cursorArrowLeft;
        public Texture2D cursorArrowRight;

        CursorArrow currentCursor = CursorArrow.DEFAULT;
        enum CursorArrow
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            DEFAULT
        }

        void Start()
        {
            instance = this;
            newPosition = transform.position;
            movementSpeed = normalSpeed;
            transform.position = startCameraTransform.position;
        }

        void Update()
        {
            if (followTransform != null)
            {
                transform.position = followTransform.position;
            }
            else
            {
                HandleCameraMovement();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                followTransform = null;
            }

            if (cameraConfiner != null)
            {
                Bounds bounds = cameraConfiner.bounds;
                newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
                newPosition.z = Mathf.Clamp(newPosition.z, bounds.min.z, bounds.max.z);
            }
        }

        void HandleCameraMovement()
        {

            if (moveWithKeyboad)
            {
                if (Input.GetKey(KeyCode.LeftCommand))
                {
                    movementSpeed = fastSpeed;
                }
                else
                {
                    movementSpeed = normalSpeed;
                }

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    newPosition += (transform.forward * movementSpeed);
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    newPosition += (transform.forward * -movementSpeed);
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    newPosition += (transform.right * movementSpeed);
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    newPosition += (transform.right * -movementSpeed);
                }
            }

            if (moveWithEdgeScrolling)
            {

                
                if (Input.mousePosition.x > Screen.width - edgeSize)
                {
                    newPosition += (transform.right * movementSpeed);
                    ChangeCursor(CursorArrow.RIGHT);
                    isCursorSet = true;
                }

                
                else if (Input.mousePosition.x < edgeSize)
                {
                    newPosition += (transform.right * -movementSpeed);
                    ChangeCursor(CursorArrow.LEFT);
                    isCursorSet = true;
                }

                
                else if (Input.mousePosition.y > Screen.height - edgeSize)
                {
                    newPosition += (transform.forward * movementSpeed);
                    ChangeCursor(CursorArrow.UP);
                    isCursorSet = true;
                }

                
                else if (Input.mousePosition.y < edgeSize)
                {
                    newPosition += (transform.forward * -movementSpeed);
                    ChangeCursor(CursorArrow.DOWN);
                    isCursorSet = true;
                }
                else
                {
                    if (isCursorSet)
                    {
                        ChangeCursor(CursorArrow.DEFAULT);
                        isCursorSet = false;
                    }
                }
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSensitivity);

            Cursor.lockState = CursorLockMode.Confined; // If we have an extra monitor we don't want to exit screen bounds
        }

        private void ChangeCursor(CursorArrow newCursor)
        {
            if (currentCursor != newCursor)
            {
                switch (newCursor)
                {
                    case CursorArrow.UP:
                        Cursor.SetCursor(cursorArrowUp, Vector2.zero, CursorMode.Auto);
                        break;
                    case CursorArrow.DOWN:
                        Cursor.SetCursor(cursorArrowDown, new Vector2(cursorArrowDown.width, cursorArrowDown.height), CursorMode.Auto); // So the Cursor will stay inside view
                        break;
                    case CursorArrow.LEFT:
                        Cursor.SetCursor(cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                        break;
                    case CursorArrow.RIGHT:
                        Cursor.SetCursor(cursorArrowRight, new Vector2(cursorArrowRight.width, cursorArrowRight.height), CursorMode.Auto); // So the Cursor will stay inside view
                        break;
                    case CursorArrow.DEFAULT:
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        break;
                }

                currentCursor = newCursor;
            }
        }


    }
}
