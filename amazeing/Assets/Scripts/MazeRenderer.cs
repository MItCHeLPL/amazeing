using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeRenderer : MonoBehaviour
{
    //Maze size
    [SerializeField] [Range(5, 50)] private int minMazeSize = 5;
    [SerializeField] [Range(5, 50)] private int maxMazeSize = 25;

    //Maze size is based on size curve rather than set maze size
    public bool sizeBasedOnSizeCurve = true;
    [SerializeField] private AnimationCurve sizeCurve;

    //if true use mazeSize and generate square maze rather than seperating width and height
    [SerializeField] private bool UseMazeSize = true;
    [Range(5, 50)] public int mazeSize = 10;

    [SerializeField] [Range(5, 50)] private int mazeWidth = 10;
    [SerializeField] [Range(5, 50)] private int mazeHeight = 10;

    //Random maze generation seed
    public int mazeSeed = 1;

    [HideInInspector] public int lvlCount = 99;

    //wall size multiplier, best at 1.0f
    [SerializeField] private float wallSize = 1.0f;

    //start game in normal/hard mode
    public bool normalMode = true;

    //draw maze on game start rather than on call
    [SerializeField] private bool drawOnStart = false;

    //Colorize maze walls
    [SerializeField] private bool colorize = true;

    //Prefabs
    [SerializeField] private Camera cam = null;
    [SerializeField] private Transform wallPrefab = null; 
    [SerializeField] private Transform finishPrefab = null; //Optional
    [SerializeField] private Transform keyPrefab = null; //Optional
    [SerializeField] private Transform playerPrefab = null; //Optional
    [SerializeField] private Transform aiPrefab = null; //Optional
    [SerializeField] private TextMeshProUGUI lvlLabel = null; //Optional
    

    //Offsets values of maze borders for calculating camera offset, allows centering of maze
    private float lookAtXOffsetLeft = 0;
    private float lookAtXOffsetRight = 0;
    private float lookAtYOffsetTop = 0;
    private float lookAtYOffsetBottom = 0;

    //Key placement
    private float x = 0;
    private int y1 = 0;
    private int y2 = 0;
    private int z1 = 0;
    private int z2 = 0;

    //Finish line
    private Transform finish;
    [SerializeField] private Color enabledFinishColor;

    //Game Timer
    [HideInInspector] public float gameTime = 0.0f;
    private IEnumerator gameTimer;

    void Start()
    {
        //make maze always squared
        if(UseMazeSize)
		{
            mazeWidth = mazeSize;
            mazeHeight = mazeSize;
        }

        //Draw maze at start
        if (drawOnStart)
		{
            var maze = MazeGenerator.Generate(mazeWidth, mazeHeight, mazeSeed);
            Draw(maze);
        }
    }

    private void Draw(WallState[,] maze)
    {
        /*Maze Drawing i - width, j - height
        * i=0 - left
        * i=mazeWidth - right
        * j=0 - top
        * j=mazeHeight - bottom*/

        if(keyPrefab != null)
		{
            //Randomize position for key placement
            Random.InitState(mazeSeed); //always same position per level
            x = Random.Range(0.0f, 1.0f);
            y1 = (int)Mathf.Round(Random.Range(0, Mathf.Round(mazeWidth / 2)));

            Random.InitState(mazeSeed*3);
            y2 = (int)Mathf.Round(Random.Range(0, Mathf.Round(mazeWidth / 2)));

            Random.InitState(mazeSeed);
            z1 = (int)Mathf.Round(Random.Range(Mathf.Round(mazeWidth / 2), mazeWidth - 1));

            Random.InitState(mazeSeed*3);
            z2 = (int)Mathf.Round(Random.Range(Mathf.Round(mazeWidth / 2), mazeWidth - 1));

            Random.InitState(mazeSeed);
        }

        for (int i = 0; i < mazeWidth; ++i)
        {
            for (int j = 0; j < mazeHeight; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-mazeWidth / 2 + i, 0, -mazeHeight / 2 + j);

                //Instantiate finish line
                //Top right
                if (i == mazeWidth - 2 && j == 0 && finishPrefab != null)
                {
                    finish = Instantiate(finishPrefab, transform) as Transform;
                    finish.localPosition = position + new Vector3(0, 0, -wallSize / 2);
                    finish.localScale = new Vector3(finish.localScale.x, wallSize, finish.localScale.z);
                    finish.localEulerAngles = new Vector3(90, 90, 0);

                    //Disable finish 
                    finish.CompareTag("Wall"); //Disable until player collects key
                    finish.GetComponent<ColorRandomizer>().RandomizeColor(mazeSeed);//Hide finish
                }
                else
                {
                    //Instantiate player
                    //Bottom left
                    if(i == 0 && j == mazeHeight - 1 && playerPrefab != null)
                    {
                        var player = Instantiate(playerPrefab, transform) as Transform;
                        player.localPosition = position + new Vector3(0, 0, 0);
                        player.localEulerAngles = new Vector3(90, 0, 0);

                        if(normalMode == false)
						{
                            var ai = Instantiate(aiPrefab, transform) as Transform;
                            ai.localPosition = position + new Vector3(0, 0, 0);
                            ai.localEulerAngles = new Vector3(90, 0, 0);
                        }
                    }

                    //Instantiate key
                    /*Random Top left/Middle/Bottom right
                    10 | if x <= 0.5
                    01 | if x > 0.5
                    (width: 0:size/2(y) && height: 0:size/2(y) && x <= 0.5)  ||  (width: size/2:size(z) && height: size/2:size(z) && x > 0.5)*/
                    if(keyPrefab != null)
					{
                        bool b1 = i == y1 && j == y2 && x <= 0.5f;
                        bool b2 = i == z1 && j == z2 && x > 0.5f;
                        if (b1 || b2)
                        {
                            var key = Instantiate(keyPrefab, transform) as Transform;
                            key.localPosition = position + new Vector3(0, 0, 0);
                            key.localEulerAngles = new Vector3(90, 0, 0);
                        }
                    }

                    //Place walls
                    if (cell.HasFlag(WallState.UP))
                    {
                        var topWall = Instantiate(wallPrefab, transform) as Transform;
                        topWall.localPosition = position + new Vector3(0, 0, wallSize / 2);
                        topWall.localScale = new Vector3(topWall.localScale.x, wallSize, topWall.localScale.z);
                        topWall.localEulerAngles = new Vector3(90, 90, 0);

                        //Color each wall
                        if (colorize)
                        {
                            topWall.GetComponent<ColorRandomizer>().RandomizeColor(mazeSeed);
                        }

                        //Save top maze border offset
                        if (i == 0 && j == mazeHeight - 1)
                        {
                            lookAtYOffsetTop = topWall.localPosition.z;
                        }
                    }

                    if (cell.HasFlag(WallState.LEFT))
                    {
                        var leftWall = Instantiate(wallPrefab, transform) as Transform;
                        leftWall.localPosition = position + new Vector3(-wallSize / 2, 0, 0);
                        leftWall.localScale = new Vector3(leftWall.localScale.x, wallSize, leftWall.localScale.z);
                        leftWall.localEulerAngles = new Vector3(90, 0, 0);

                        //Color each wall
                        if (colorize)
                        {
                            leftWall.GetComponent<ColorRandomizer>().RandomizeColor(mazeSeed);
                        }

                        //Save left maze border offset
                        if (i == 0 && j == 0)
						{
                            lookAtXOffsetLeft = leftWall.localPosition.x;
                        }
                    }

                    if (i == mazeWidth - 1)
                    {
                        if (cell.HasFlag(WallState.RIGHT))
                        {
                            var rightWall = Instantiate(wallPrefab, transform) as Transform;
                            rightWall.localPosition = position + new Vector3(+wallSize / 2, 0, 0);
                            rightWall.localScale = new Vector3(rightWall.localScale.x, wallSize, rightWall.localScale.z);
                            rightWall.localEulerAngles = new Vector3(90, 0, 0);

                            //Color each wall
                            if (colorize)
                            {
                                rightWall.GetComponent<ColorRandomizer>().RandomizeColor(mazeSeed);
                            }

                            //Save right maze border offset
                            if (i == mazeWidth - 1 && j == 0)
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
                            bottomWall.localPosition = position + new Vector3(0, 0, -wallSize / 2);
                            bottomWall.localScale = new Vector3(bottomWall.localScale.x, wallSize, bottomWall.localScale.z);
                            bottomWall.localEulerAngles = new Vector3(90, 90, 0);

                            //Color each wall
                            if (colorize)
							{
                                bottomWall.GetComponent<ColorRandomizer>().RandomizeColor(mazeSeed);
                            }

                            //Save bottom maze border offset
                            if (i == 0 && j == 0)
                            {
                                lookAtYOffsetBottom = bottomWall.localPosition.z;
                            }
                        }
                    }
                }
            }
        }

        //Whole maze is drawn
        if (normalMode == false)
        {
            Invoke("RecalculateAIPathfinding", .5f);
        }
    }

    public void StartGame(int lvlNumber, int lvlAmount, bool normal)
	{
        if(sizeBasedOnSizeCurve && UseMazeSize)
		{
            //linear = newMin + (val - minVal) * (newMax - newMin) / (maxVal - minVal);
            //mazeSize = minMazeSize + (lvlNumber - 1) * (maxMazeSize - minMazeSize) / (lvlAmount - 1); //linear scale
            mazeSize = (int)(minMazeSize + (sizeCurve.Evaluate((float)lvlNumber / (float)lvlAmount) - 0) * (maxMazeSize - minMazeSize) / (1 - 0)); //curve evaluation of maze scale

            Debug.Log("Level Number: " + lvlNumber + ", Maze size: " + mazeSize + ", Curve value: " + sizeCurve.Evaluate((float)lvlNumber / (float)lvlAmount)); //Debug
        }

        if(UseMazeSize)
		{
            mazeWidth = mazeSize;
            mazeHeight = mazeSize;
        }

        normalMode = normal; //normal/hard game mode
        mazeSeed = lvlNumber; //use lvlNumer as maze seed
        lvlCount = lvlAmount;

        if(lvlLabel != null)
		{
            lvlLabel.SetText(lvlNumber.ToString()); //Display lvl number
        }

        var maze = MazeGenerator.Generate(mazeWidth, mazeHeight, mazeSeed); //Generate maze

        Draw(maze); //Draw maze

        //Start counting the time
        gameTimer = GameTimer();
        StartCoroutine(gameTimer);

        //change camera view size to fit entire maze inside
        cam.orthographicSize = mazeSize + 2.0f;

        //This fixes centering by offseting camera X and Y axis by difference between maze outer walls position
        cam.transform.position = new Vector3((lookAtXOffsetLeft + lookAtXOffsetRight) * 0.5f, mazeSize % 2 == 0 ? (lookAtYOffsetTop + lookAtYOffsetBottom) * 0.5f : (lookAtYOffsetTop + lookAtYOffsetBottom) * 0.5f - 0.5f, cam.transform.position.z);
    }

    public void PauseGame()
	{
        StopCoroutine(gameTimer);
	}
    public void ResumeeGame()
    {
        gameTimer = GameTimer();
        StartCoroutine(gameTimer);
    }

    public void StopGame()
	{
        //Stop game timer
        StopCoroutine(gameTimer);
    }

    private IEnumerator GameTimer()
	{
        while(true)
		{
            gameTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void UnlockFinish()
	{
        finish.CompareTag("Finish"); //Enable finish

        finish.GetComponent<SpriteRenderer>().color = enabledFinishColor; //Change color to enabled
    }

    public void ClearGame()
    {
        StopGame();

        //Clear maze
        for (int i=0; i < transform.childCount; i++)
		{
            Destroy(transform.GetChild(i).gameObject);
		}

        //Reset game timer
        gameTime = 0.0f;

        //main menu background maze, 12 is best for menu
        cam.orthographicSize = 12; 
    }

    public void RecalculateAIPathfinding()
	{
        AstarPath.active.Scan();
	}
}
