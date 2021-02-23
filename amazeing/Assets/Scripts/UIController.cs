using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIController : MonoBehaviour
{
	private PlayerController player;
	[SerializeField] private MazeRenderer gameplayMazeRenderer;
	public GameMenager gameMenager;

	//Panels
	[SerializeField] private GameObject gameplayPanel;
	[SerializeField] private GameObject menuPanel;

	//gameplay
	[SerializeField] private GameObject gameplayoverlayPanel;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject endgamePanel;

	public GameObject keyIconEnabled;
	public GameObject keyIconDisabled;

	public TextMeshProUGUI scoreValue;
	public TextMeshProUGUI endgameScoreValue;
	public TextMeshProUGUI pauseScoreValue;

	public TextMeshProUGUI endgameTimeValue;
	public TextMeshProUGUI pauseTimeValue;

	public TextMeshProUGUI lvlLabel;

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

	public void ClearLevel()
	{
		EnablePanel(keyIconDisabled);
		DisablePanel(keyIconEnabled);

		//Clear score counters
		UpdateScoreValues(0);

		//Clear time counters
		UpdateTimeValues(0);
	}

	public void UpdateTimeValues(float time)
	{
		//Round to 2 decimap places
		time = (Mathf.Round(time * 100)) / 100.0f;

		//Update value text
		pauseTimeValue.SetText(time.ToString() + " sec");
		endgameTimeValue.SetText(time.ToString() + " sec");
	}

	public void UpdateScoreValues(float score)
	{
		scoreValue.SetText(score.ToString());
		endgameScoreValue.SetText(score.ToString());
		pauseScoreValue.SetText(score.ToString());
	}

	public void UpdateLVLCounter(int lvlNumber)
	{
		lvlLabel.SetText(lvlNumber.ToString());
	}
}