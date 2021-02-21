using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIController : MonoBehaviour
{
	private PlayerController player;
	[SerializeField] private GameObject gameplayMazeRenderer;

	//Panels
	[SerializeField] private GameObject gameplayPanel;
	[SerializeField] private GameObject menuPanel;

	//gameplay
	[SerializeField] private GameObject gameplayoverlayPanel;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject endgamePanel;

	public GameObject keyIconEnabled;
	public GameObject keyIconDisabled;

	public GameObject scoreValue;
	public GameObject endgameScoreValue;
	public GameObject pauseScoreValue;
	public GameObject endgameTimeValue;
	public GameObject pauseTimeValue;

	//menu
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject playPanel;
	[SerializeField] private GameObject aboutPanel;

	//play
	[SerializeField] private GameObject normalPanel;
	[SerializeField] private GameObject hardPanel;


	//Set player component on runtime after player is generated
	public void SetPlayerComponent(GameObject go)
	{
		player = go.GetComponent<PlayerController>();
	}


	//Panel switching
	public void EnablePanel(GameObject panel)
	{
		panel.SetActive(true);
	}
	public void DisablePanel(GameObject panel)
	{
		panel.SetActive(false);
	}


	//Player control buttons
	public void LeftAction()
	{
		player.MovePlayer(new Vector2(-1, 0));
	}

	public void UpAction()
	{
		player.MovePlayer(new Vector2(0, 1));
	}

	public void DownAction()
	{
		player.MovePlayer(new Vector2(0, -1));
	}

	public void RightAction()
	{
		player.MovePlayer(new Vector2(1, 0));
	}


	//Level handling
	public void EndGameAction()
	{
		gameplayMazeRenderer.GetComponent<MazeRenderer>().StopGame();

		UpdateTimeValues();

		EnablePanel(endgamePanel);
		DisablePanel(gameplayoverlayPanel);
	}

	public void StartLevel(int lvlNumber, int levelAmount, bool normal)
	{
		gameplayMazeRenderer.GetComponent<MazeRenderer>().StartGame(lvlNumber, levelAmount, normal);

		EnablePanel(gameplayPanel);
		EnablePanel(gameplayoverlayPanel);
		DisablePanel(menuPanel);
		DisablePanel(mainPanel);
	}

	public void StartNextLevel()
	{
		int lvl = gameplayMazeRenderer.GetComponent<MazeRenderer>().mazeSeed;
		int lvlCount = gameplayMazeRenderer.GetComponent<MazeRenderer>().lvlCount;
		bool normal = gameplayMazeRenderer.GetComponent<MazeRenderer>().normalMode;

		ClearLevel();

		StartLevel(lvl+1, lvlCount, normal);
	}

	public void RestartLevel()
	{
		int lvl = gameplayMazeRenderer.GetComponent<MazeRenderer>().mazeSeed;
		int lvlCount = gameplayMazeRenderer.GetComponent<MazeRenderer>().lvlCount;
		bool normal = gameplayMazeRenderer.GetComponent<MazeRenderer>().normalMode;

		ClearLevel();

		StartLevel(lvl, lvlCount, normal);
	}

	public void ClearLevel()
	{
		gameplayMazeRenderer.GetComponent<MazeRenderer>().ClearGame();

		EnablePanel(keyIconDisabled);
		DisablePanel(keyIconEnabled);

		//Clear score counters
		scoreValue.GetComponent<TextMeshProUGUI>().SetText("0");
		endgameScoreValue.GetComponent<TextMeshProUGUI>().SetText("0");

		//Clear time counters
		pauseTimeValue.GetComponent<TextMeshProUGUI>().SetText("0");
		endgameTimeValue.GetComponent<TextMeshProUGUI>().SetText("0");
	}

	public void UpdateTimeValues()
	{
		//Get Time
		float time = gameplayMazeRenderer.GetComponent<MazeRenderer>().gameTime;

		//Round to 2 decimap places
		time = (Mathf.Round(time * 100)) / 100.0f;

		//Update value text
		pauseTimeValue.GetComponent<TextMeshProUGUI>().SetText(time.ToString() + " sec");
		endgameTimeValue.GetComponent<TextMeshProUGUI>().SetText(time.ToString() + " sec");
	}
}