using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIController : MonoBehaviour
{
	//Get Scripts
	private PlayerController player;
	[SerializeField] private MazeRenderer gameplayMazeRenderer;
	public GameMenager gameMenager;

	//Panels
	[SerializeField] private GameObject gameplayPanel;
	[SerializeField] private GameObject menuPanel;

	//Gameplay
	[SerializeField] private GameObject gameplayoverlayPanel;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject endgamePanel;

	//Score
	public TextMeshProUGUI scoreValue;
	public TextMeshProUGUI endgameScoreValue;
	public TextMeshProUGUI pauseScoreValue;

	//Time
	public TextMeshProUGUI endgameTimeValue;
	public TextMeshProUGUI pauseTimeValue;

	//Lvl
	public TextMeshProUGUI lvlLabel;

	//Menu Panels
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject playPanel;
	[SerializeField] private GameObject aboutPanel;

	//Play Panels
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


	//Player navigation buttons
	public void LeftAction()
	{
		player.MovePlayer(Vector2.left);
	}

	public void UpAction()
	{
		player.MovePlayer(Vector2.up);
	}

	public void DownAction()
	{
		player.MovePlayer(Vector2.down);
	}

	public void RightAction()
	{
		player.MovePlayer(Vector2.right);
	}


	//UI Level handling
	public void EndGameAction()
	{
		EnablePanel(endgamePanel);
		DisablePanel(gameplayoverlayPanel);
	}

	public void StartLevel()
	{
		EnablePanel(gameplayPanel);
		EnablePanel(gameplayoverlayPanel);
		DisablePanel(menuPanel);
		DisablePanel(mainPanel);
	}

	//TODO Move to separate script HelpController with cost, separate panel etc.
	public void Help()
	{
		var ai = gameplayMazeRenderer.ai.GetComponent<AIController>();

		//TODO add cost to using help

		ai.ShowPath();
	}
	

	//Clear UI after lvl end
	public void ClearLevel()
	{
		//Clear score counters
		UpdateScoreValues(0);

		//Clear time counters
		UpdateTimeValues(0);
	}

	//Refresh Time
	public void UpdateTimeValues(float time)
	{
		//Round to 2 decimap places
		time = (Mathf.Round(time * 100)) / 100.0f;

		//Update value text
		pauseTimeValue.SetText(time.ToString() + " sec");
		endgameTimeValue.SetText(time.ToString() + " sec");
	}

	//Refresh Score
	public void UpdateScoreValues(float score)
	{
		scoreValue.SetText(score.ToString());
		endgameScoreValue.SetText(score.ToString());
		pauseScoreValue.SetText(score.ToString());
	}

	//Refresh lvl label
	public void UpdateLVLCounter(int lvlNumber)
	{
		lvlLabel.SetText(lvlNumber.ToString());
	}
}