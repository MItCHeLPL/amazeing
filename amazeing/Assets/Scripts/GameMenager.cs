using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameMenager : MonoBehaviour
{
	public UIController uiController = null;
	public MazeRenderer mazeRenderer = null;
	public CameraController camController = null;

    [SerializeField] private Slider raceModeTimer = null; //Slider form race mode timer

    [SerializeField] private GameObject loadingScreen = null;

    public int lvlCount = 100; //Total lvl count
    public int ClampMaxMazeSizeToLvl = 100; //every level after this value uses max maze size
    private bool normalGameMode = true; //Normal/Race mode

    [HideInInspector] public int lastUnlockedLvl = 1;
    [HideInInspector] public int lastRaceUnlockedLvl = 1;

    public int score = 0; //Player score
    public int pathLength = 0; //Total path to finish through key position
    
    public float gameTime = 0.0f; //Game Timer
    private IEnumerator gameTimer; //Game Timer Coroutine

    [SerializeField] private float raceModeTimeMultiplier = 0.66f; //Multiply time that player has to finish lvl in race mode

    [SerializeField] private UnityEvent OnWin;
    [SerializeField] private UnityEvent OnLoose;

	private void Start()
	{
        LoadPlayerPrefs();
    }

	//Start Level
	public void StartGame(int lvlNumber, bool normal)
	{
        StartCoroutine(StartGameCoroutine(lvlNumber, normal)); //Start game starter coroutine
    }
    private IEnumerator StartGameCoroutine(int lvlNumber, bool normal)
	{
        //Enable loading screen
        loadingScreen.SetActive(true);

        bool maze = false;
        //maze = mazeRenderer.GenerateMazeForGameplay(lvlNumber, lvlCount); //Generate maze
        maze = mazeRenderer.GenerateMazeForGameplay(lvlNumber, ClampMaxMazeSizeToLvl); //Generate maze

        yield return new WaitUntil(() => maze); //Wait until maze is fully generated

        normalGameMode = normal;

        var ai = mazeRenderer.ai.GetComponent<AIController>();

        yield return new WaitUntil(() => ai.pathCalculated); //Wait until path is calculated

        pathLength = (int)ai.pathLength; //Get path length

        //if race mode
        if (normal == false)
		{
            raceModeTimer.gameObject.SetActive(true); //enable slider
            raceModeTimer.maxValue = (raceModeTimeMultiplier * pathLength); //set max value to time that player has to finish race
        }
        else
		{
            raceModeTimer.gameObject.SetActive(false);
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

        //Disable loading screen after finished setting everything up and animation played at least once
        yield return new WaitUntil(() => loadingScreen.GetComponent<LoadingScreenAnimation>().finishedAnimationOnece);    
        loadingScreen.SetActive(false);

        //Start counting the time
        gameTimer = GameTimer();
        StartCoroutine(gameTimer);
    }

    //Reset level and start next
    public void StartNextLevel()
    {
        int lvl = mazeRenderer.mazeSeed;
        bool normal = normalGameMode;

        ClearGame();

        StartGame(lvl+1, normal);
    }

    //Reset level
    public void RestartGame()
    {
        int lvl = mazeRenderer.mazeSeed;
        bool normal = normalGameMode;

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
    public void StopGame(bool win)
	{
        //Stop game timer
        StopCoroutine(gameTimer);

        if(win)
		{
            OnWin.Invoke();

            if(lastUnlockedLvl == mazeRenderer.mazeSeed && normalGameMode)
			{
                lastUnlockedLvl++;
                SavePlayerPrefs();
            }
            else if(lastRaceUnlockedLvl == mazeRenderer.mazeSeed && !normalGameMode)
			{
                lastRaceUnlockedLvl++;
                SavePlayerPrefs();
            }
        }
        else
		{
            OnLoose.Invoke();
        }

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

            //If race mode subtract game time from path length to calculate left time
            if(normalGameMode == false)
			{
                raceModeTimer.value = (raceModeTimeMultiplier * pathLength) - gameTime; //full race time - current time

                //when times runs out
                if(raceModeTimer.value == raceModeTimer.minValue)
				{
                    StopGame(false);
                }
            }

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


    private void LoadPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("LastUnlockedLvl"))
        {
            SavePlayerPrefs();
        }
        else
		{
            lastUnlockedLvl = PlayerPrefs.GetInt("LastUnlockedLvl");
            lastRaceUnlockedLvl = PlayerPrefs.GetInt("LastRaceUnlockedLvl");
        }
    }

    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("LastUnlockedLvl", lastUnlockedLvl);
        PlayerPrefs.SetInt("LastRaceUnlockedLvl", lastRaceUnlockedLvl);

        PlayerPrefs.Save();
    }
}
