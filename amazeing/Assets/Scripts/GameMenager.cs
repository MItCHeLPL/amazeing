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
    private IEnumerator gameTimer; //Game Timer Coroutine

    public int pathLength = 0; //Total path to finish through key position

    public void StartGame(int lvlNumber, bool normal) //TODO Convert to IEnumerator
	{
        bool maze = mazeRenderer.GenerateMazeForGameplay(lvlNumber, lvlCount, normal);

        //if race mode
        if(normal == false)
		{
            //Get path length
            pathLength = (int)mazeRenderer.ai.GetComponent<AIController>().pathLength;
        }

        //UI
        if (uiController.lvlLabel != null)
        {
            uiController.EnablePanel(uiController.lvlLabel.gameObject);
            uiController.UpdateLVLCounter(lvlNumber);
        }
        uiController.StartLevel();

        //Camera controller



        //change camera view size to fit entire maze inside
        camController.cam.orthographicSize = mazeRenderer.mazeSize + 2.0f;
        //This fixes centering by offseting camera X and Y axis by difference between maze outer walls position
        camController.cam.transform.position = new Vector3((mazeRenderer.lookAtXOffsetLeft + mazeRenderer.lookAtXOffsetRight) * 0.5f, mazeRenderer.mazeSize % 2 == 0 ? (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) * 0.5f : (mazeRenderer.lookAtYOffsetTop + mazeRenderer.lookAtYOffsetBottom) * 0.5f - 0.5f, camController.cam.transform.position.z);




        //Start counting the time
        gameTimer = GameTimer();
        StartCoroutine(gameTimer);
    }

    //Reset level and start next
    public void StartNextLevel()
    {
        int lvl = mazeRenderer.mazeSeed;
        bool normal = mazeRenderer.normalMode;

        ClearGame();

        StartGame(lvl+1, normal);
    }

    //Reset level
    public void RestartGame()
    {
        int lvl = mazeRenderer.mazeSeed;
        bool normal = mazeRenderer.normalMode;

        ClearGame();

        StartGame(lvl, normal);
    }

    //Pause
    public void PauseGame()
	{
        StopCoroutine(gameTimer);

        //UI
        uiController.UpdateTimeValues(gameTime);
    }

    //Resume
    public void ResumeGame()
	{
        //Resume Timer
        gameTimer = GameTimer();
        StartCoroutine(gameTimer);
    }

    //Endgame
    public void StopGame()
	{
        //Stop game timer
        StopCoroutine(gameTimer);

        //UI
        uiController.EndGameAction();
        uiController.UpdateTimeValues(gameTime);
    }

    //Clear
    public void ClearGame()
	{
        //Reset game timer
        gameTime = 0.0f;

        //Reset score
        ChangeScore(0);

        //Reset path length
        pathLength = 0;

        //UI
        mazeRenderer.ClearMaze();
        uiController.ClearLevel();
        if (uiController.lvlLabel != null)
        {
            uiController.DisablePanel(uiController.lvlLabel.gameObject);
        }

        //Camera Controller



        //main menu background maze, 12 is best for menu
        camController.cam.orthographicSize = 12;
  
        



    }

    //Calculate Time
    private IEnumerator GameTimer()
    {
        while (true)
        {
            gameTime += Time.unscaledDeltaTime; //Calculate realtime
            yield return new WaitForEndOfFrame();
        }
    }

    //Moves
    public void ChangeScore(int x)
    {
        //Change score
        score = x;

        //Ui
        uiController.UpdateScoreValues(score);
    }
}
