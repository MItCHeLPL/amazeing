using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenager : MonoBehaviour
{
	public UIController uiController = null;
	public MazeRenderer mazeRenderer = null;
	public CameraController camController = null;

    public int lvlCount = 100; //Total lvl count
    public bool normalMode = true; //start game in normal/hard mode
    public int score = 0; //Player score

    public float gameTime = 0.0f; //Game Timer
    private IEnumerator gameTimer;

    public float pathLength = 0;

    public void StartGame(int lvlNumber, bool normal) //Convert to IEnumerator
	{
        bool maze = mazeRenderer.GenerateMazeForGameplay(lvlNumber, lvlCount, normal);

        if(normal == false)
		{
            pathLength = Mathf.Floor(mazeRenderer.ai.GetComponent<AIController>().pathLength);
        }

        if (uiController.lvlLabel != null)
        {
            uiController.EnablePanel(uiController.lvlLabel.gameObject);
            uiController.UpdateLVLCounter(lvlNumber);
        }
        uiController.StartLevel();



        //ADD CAMERA CONTROLER

        //change camera view size to fit entire maze inside
        camController.cam.orthographicSize = mazeRenderer.mazeSize + 2.0f;
        //This fixes centering by offseting camera X and Y axis by difference between maze outer walls position
        camController.cam.transform.position = new Vector3((mazeRenderer.lookAtXOffsetLeft + mazeRenderer.lookAtXOffsetRight) * 0.5f, mazeRenderer.mazeSize % 2 == 0 ? (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) * 0.5f : (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) * 0.5f - 0.5f, camController.cam.transform.position.z);




        //Start counting the time
        gameTimer = GameTimer();
        StartCoroutine(gameTimer);
    }

    public void StartNextLevel()
    {
        int lvl = mazeRenderer.mazeSeed;
        bool normal = mazeRenderer.normalMode;

        ClearGame();

        StartGame(lvl+1, normal);
    }

    public void RestartGame()
    {
        int lvl = mazeRenderer.mazeSeed;
        bool normal = mazeRenderer.normalMode;

        ClearGame();

        StartGame(lvl, normal);
    }

    public void PauseGame()
	{
        StopCoroutine(gameTimer);

        uiController.UpdateTimeValues(gameTime);
    }

    public void ResumeGame()
	{
        gameTimer = GameTimer();
        StartCoroutine(gameTimer);
    }

    public void StopGame()
	{
        //Stop game timer
        StopCoroutine(gameTimer);

        uiController.EndGameAction();

        uiController.UpdateTimeValues(gameTime);
    }

    public void ClearGame()
	{
        //Reset game timer
        gameTime = 0.0f;

        ChangeScore(0);

        mazeRenderer.ClearMaze();
        uiController.ClearLevel();


        //ADD CAMERA CONTROLER

        //main menu background maze, 12 is best for menu
        camController.cam.orthographicSize = 12;




        if (uiController.lvlLabel != null)
        {
            uiController.DisablePanel(uiController.lvlLabel.gameObject);
        }
    }

    private IEnumerator GameTimer()
    {
        while (true)
        {
            gameTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void ChangeScore(int x)
    {
        //Change score
        score = x;

        //Change Ui
        uiController.UpdateScoreValues(score);
    }
}
