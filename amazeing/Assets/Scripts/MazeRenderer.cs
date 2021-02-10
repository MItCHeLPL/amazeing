using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    [Range(5, 30)]
    private int mazeSize = 10;

    [SerializeField]
    private bool sizeBasedOnLvlAmount = true;

    [SerializeField]
    private int lvlAmount = 99;

    private int width = 10;
    private int height = 10;

    public int seed = 1;

    private float size = 1.0f;

    /*[SerializeField]
    private float GenerationAnimationDuration = 1.0f;*/

    [SerializeField]
    private Transform wallPrefab = null;

    /*[SerializeField]
    private Transform startPrefab = null;*/

    [SerializeField]
    private Transform finishPrefab = null;

    [SerializeField]
    private Transform playerPrefab = null;

    private float lookAtXOffsetLeft = 0;
    private float lookAtXOffsetRight = 0;
    private float lookAtYOffsetTop = 0;
    private float lookAtYOffsetBottom = 0;

    [SerializeField]
    private Camera cam = null;

    /*[SerializeField]
    private Transform keyPrefab = null;*/

    public bool normalMode = true;

    [SerializeField]
    private TextMeshProUGUI lvlLabel = null;

    [SerializeField]
    private bool drawOnStart = false;
    [SerializeField]
    private bool colorize = true;

    void Start()
    {
        width = mazeSize;
        height = mazeSize;

        if (drawOnStart)
		{
            var maze = MazeGenerator.Generate(width, height, seed);
            Draw(maze);
        }
    }

    private void Draw(WallState[,] maze)
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if (i == width - 2 && j == 0 && finishPrefab != null)
                {
                    //open finish top right
                    var finish = Instantiate(finishPrefab, transform) as Transform;
                    finish.localPosition = position + new Vector3(0, 0, -size / 2);
                    finish.localScale = new Vector3(finish.localScale.x, size, finish.localScale.z);
                    finish.localEulerAngles = new Vector3(90, 90, 0);
                }
                /*else if (i == 1 && j == height - 1)
                {
                    //Start
                    //open enterence bottom left
                    var start = Instantiate(startPrefab, transform) as Transform;
                    start.localPosition = position + new Vector3(0, 0, size / 2);
                    start.localScale = new Vector3(start.localScale.x, size, start.localScale.z);
                    start.localEulerAngles = new Vector3(90, 90, 0);

                    //place player
                    var player = Instantiate(playerPrefab, transform) as Transform;
                    player.localPosition = position + new Vector3(0, 0, 0);
                    player.localEulerAngles = new Vector3(90, 0, 0);

                }*/
                else
                {
                    if(i == 0 && j == height - 1 && playerPrefab != null)
                    {
                        //place player
                        var player = Instantiate(playerPrefab, transform) as Transform;
                        player.localPosition = position + new Vector3(0, 0, 0);
                        player.localEulerAngles = new Vector3(90, 0, 0);
                    }

                    //place walls
                    if (cell.HasFlag(WallState.UP))
                    {
                        var topWall = Instantiate(wallPrefab, transform) as Transform;
                        topWall.localPosition = position + new Vector3(0, 0, size / 2);
                        topWall.localScale = new Vector3(topWall.localScale.x, size, topWall.localScale.z);
                        topWall.localEulerAngles = new Vector3(90, 90, 0);
                        if (colorize)
                        {
                            topWall.GetComponent<ColorRandomizer>().RandomizeColor(seed);
                        }

                        if (i == 0 && j == height - 1)
                        {
                            lookAtYOffsetTop = topWall.localPosition.z;
                        }
                    }

                    if (cell.HasFlag(WallState.LEFT))
                    {
                        var leftWall = Instantiate(wallPrefab, transform) as Transform;
                        leftWall.localPosition = position + new Vector3(-size / 2, 0, 0);
                        leftWall.localScale = new Vector3(leftWall.localScale.x, size, leftWall.localScale.z);
                        leftWall.localEulerAngles = new Vector3(90, 0, 0);
                        if (colorize)
                        {
                            leftWall.GetComponent<ColorRandomizer>().RandomizeColor(seed);
                        }

                        if (i == 0 && j == 0)
						{
                            lookAtXOffsetLeft = leftWall.localPosition.x;
                        }
                    }

                    if (i == width - 1)
                    {
                        if (cell.HasFlag(WallState.RIGHT))
                        {
                            var rightWall = Instantiate(wallPrefab, transform) as Transform;
                            rightWall.localPosition = position + new Vector3(+size / 2, 0, 0);
                            rightWall.localScale = new Vector3(rightWall.localScale.x, size, rightWall.localScale.z);
                            rightWall.localEulerAngles = new Vector3(90, 0, 0);
                            if (colorize)
                            {
                                rightWall.GetComponent<ColorRandomizer>().RandomizeColor(seed);
                            }

                            if (i == width - 1 && j == 0)
                            {
                                lookAtXOffsetRight = rightWall.localPosition.x;
                            }
                        }
                    }

                    if (j == 0)
                    {
                        if (cell.HasFlag(WallState.DOWN))
                        {
                            var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                            bottomWall.localPosition = position + new Vector3(0, 0, -size / 2);
                            bottomWall.localScale = new Vector3(bottomWall.localScale.x, size, bottomWall.localScale.z);
                            bottomWall.localEulerAngles = new Vector3(90, 90, 0);
                            if(colorize)
							{
                                bottomWall.GetComponent<ColorRandomizer>().RandomizeColor(seed);
                            }
                            

                            if (i == 0 && j == 0)
                            {
                                lookAtYOffsetBottom = bottomWall.localPosition.z;
                            }
                        }
                    }
                }

                //yield return new WaitForSeconds((GenerationAnimationDuration / width) / height); //load maze for set amount of seconds
            }
        }
    }

    public void StartGame(int lvlNumber, bool normal)
	{
        if(sizeBasedOnLvlAmount)
		{
            //TODO add curvature to this
            //alpha = newMin + (val - minVal) * (newMax - newMin) / (maxVal - minVal);
            mazeSize = 4 + (lvlNumber - 1) * (30 - 4) / (lvlAmount - 1);
        }

        width = mazeSize;
        height = mazeSize;

        normalMode = normal;
        seed = lvlNumber;

        lvlLabel.SetText(lvlNumber.ToString());

        var maze = MazeGenerator.Generate(width, height, seed);

        Draw(maze);

        //fit whole maze into screen with margin
        cam.orthographicSize = mazeSize + 0.5f;

        //This fixes centering by offseting camera X axis by difference between maze outer walls x position
        //fix Y offset
        cam.transform.position = new Vector3((lookAtXOffsetLeft + lookAtXOffsetRight) * 0.5f, mazeSize % 2 == 0 ? (lookAtYOffsetTop + lookAtYOffsetBottom) * 0.5f : (lookAtYOffsetTop + lookAtYOffsetBottom) * 0.5f - 0.5f, cam.transform.position.z);
    }

    public void ClearGame()
    {
        for(int i=0; i < transform.childCount; i++)
		{
            Destroy(transform.GetChild(i).gameObject);
		}
    }
}
