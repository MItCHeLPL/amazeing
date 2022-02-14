using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private MazeRenderer mazeRenderer;

    public Camera cam; //temp until I get camera to work on cinemachine

    public void EnableMenuCamera()
	{
        //temp until I get camera to work on cinemachine
        //main menu background maze, 12 is best for menu
        cam.orthographicSize = 12;
    }

    public void EnableGameplayCamera()
	{
        //temp until I get camera to work on cinemachine
        //change camera view size to fit entire maze inside
        cam.orthographicSize = mazeRenderer.mazeSize + 2.0f;

        //This fixes centering by offseting camera X and Y axis by difference between maze outer walls position
        cam.transform.position = new Vector3((mazeRenderer.lookAtXOffsetLeft + mazeRenderer.lookAtXOffsetRight) * 0.5f, mazeRenderer.mazeSize % 2 == 0 ? (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) * 0.5f : (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) * 0.5f - 0.5f, cam.transform.position.z);
    }

    //TODO Implement:

    //Author: https://twitter.com/astoldbywaldo/
    //Fitment
    /*public SpriteRenderer rink;

	// Use this for initialization
	void Start () {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = rink.bounds.size.x / rink.bounds.size.y;

        if(screenRatio >= targetRatio){
            Camera.main.orthographicSize = rink.bounds.size.y / 2;
        }else{
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = rink.bounds.size.y / 2 * differenceInSize;
        }
	}*/

    //Author: https://twitter.com/astoldbywaldo/
    //Pinch Zoom
    /*Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)){
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.touchCount == 2){
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }else if(Input.GetMouseButton(0)){
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
	}

    void zoom(float increment){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }*/
}
