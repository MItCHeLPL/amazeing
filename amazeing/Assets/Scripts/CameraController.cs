using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CameraController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private MazeRenderer mazeRenderer;
    private Camera cam;

    private bool isInGameplay = false;
    private bool isTouchStartInGameplayPanel = false;

    [Header("Menu")]
    [SerializeField] private float camSizeInMenu = 12.0f;


    [Header("Gameplay")]
    [SerializeField] private float mazeMargin = 3.0f;

    //Zoom
    private Vector3 touchStart = Vector3.zero;
    [SerializeField] private float zoomOutMin = 6.0f;
    private float zoomOutMax = 12.0f;


	private void Start()
	{
        cam = transform.GetComponent<Camera>();
    }


	public void EnableMenuCamera()
	{
        isInGameplay = false;

        //main menu background maze, (12 is best for menu)
        cam.orthographicSize = camSizeInMenu;
    }

    public void EnableGameplayCamera()
	{
        isInGameplay = true;

        //change camera view size to fit entire maze inside
        cam.orthographicSize = mazeRenderer.mazeSize + mazeMargin;

        zoomOutMax = mazeRenderer.mazeSize + mazeMargin;

        //Center camera
        cam.transform.position = new Vector3(
            (mazeRenderer.lookAtXOffsetLeft + mazeRenderer.lookAtXOffsetRight) / 2, 
            mazeRenderer.mazeSize % 2 == 0 ? (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) / 2 : (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) / 2 - 0.5f, 
            cam.transform.position.z
        );
    }


	private void Update () 
    {
        if (isInGameplay)
        {
            //Based on: https://twitter.com/astoldbywaldo/

            //Screen Input
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = cam.ScreenToWorldPoint(Input.mousePosition);

                //If input in gameplay panel
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                var raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);

                if (raycastResults.Count > 0)
                {
                    foreach (var result in raycastResults)
                    {
                        isTouchStartInGameplayPanel = result.gameObject.CompareTag("GameplayPanel");

                        if (isTouchStartInGameplayPanel)
						{
                            break; 
						}
                    }
                }
            }
        
            //TODO check how zoom works on mobile and maybe add isTouchStartInGameplayPanel to if
            //Touch input zoom
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                ZoomCamera(difference * 0.01f);
            }

            //Moving camera
            else if (Input.GetMouseButton(0) && isTouchStartInGameplayPanel)
            {
                Vector3 direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);

                Vector3 newPos = cam.transform.position + direction;

                //Clamping to outer maze edges + margin
                newPos = new Vector3(
                    Mathf.Clamp(newPos.x, mazeRenderer.lookAtXOffsetLeft + mazeMargin, mazeRenderer.lookAtXOffsetRight - mazeMargin),
                    Mathf.Clamp(newPos.y, mazeRenderer.lookAtYOffsetBottom + mazeMargin, mazeRenderer.lookAtYOffsetTop - mazeMargin),
                    newPos.z
                );

                cam.transform.position = newPos;
            }

            //Mouse input zoom
            if(Input.GetAxis("Mouse ScrollWheel") != 0.0f)
		    {
                ZoomCamera(Input.GetAxis("Mouse ScrollWheel") * 4.0f);
            }
        }
    }
   

    private void ZoomCamera(float increment)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
