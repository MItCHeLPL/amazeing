using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenager : MonoBehaviour
{
	public UIController uiController = null;
	public MazeRenderer mazeRenderer = null;
	public CameraController camController = null;

    public int lvlCount = 100; //Total lvl count
    
    public int score = 0; //Player score

    public float gameTime = 0.0f; //Game Timer
    private IEnumerator gameTimer; //Game Timer Coroutine

    public int pathLength = 0; //Total path to finish through key position

    //Start Level
    public void StartGame(int lvlNumber, bool normal)
	{
        StartCoroutine(StartGameCoroutine(lvlNumber, normal)); //Start game starter coroutine
    }
    private IEnumerator StartGameCoroutine(int lvlNumber, bool normal)
	{

        //TODO ENABLE LOADING SCREEN


        bool maze = false;
        maze = mazeRenderer.GenerateMazeForGameplay(lvlNumber, lvlCount, normal); //Generate maze

        yield return new WaitUntil(() => maze); //Wait until maze is fully generated

        //if race mode
        if (normal == false)
		{
            mazeRenderer.RecalculateAIPathfinding(); //Calculate Path

            yield return new WaitUntil(() => mazeRenderer.ai.GetComponent<AIController>().pathCalculated); //Wait until path is calculated
            
            pathLength = (int)mazeRenderer.ai.GetComponent<AIController>().pathLength; //Get path length
        }

        //UI
        if (uiController.lvlLabel != null)
        {
            uiController.EnablePanel(uiController.lvlLabel.gameObject);
            uiController.UpdateLVLCounter(lvlNumber);
        }
        uiController.StartLevel();

        //Camera controller
        camController.EnableGameplayCamera(); //Enable gameplay camera settings


        //TODO DISABLE LOADING SCREEN


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
        camController.EnableMenuCamera(); //Set Camera to menu settings
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
