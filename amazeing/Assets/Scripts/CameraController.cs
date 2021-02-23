using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

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
}
