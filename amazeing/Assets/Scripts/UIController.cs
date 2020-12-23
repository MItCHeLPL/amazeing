using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
	private PlayerController player;
	[SerializeField] private GameObject gameplayMazeRenderer;

	private GameObject prevPanel;
	private GameObject activePanel;

	//Panels
	[SerializeField] private GameObject gameplayPanel;
	[SerializeField] private GameObject menuPanel;

	//gameplay
	[SerializeField] private GameObject gameplayoverlayPanel;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject endgamePanel;

	[SerializeField] private GameObject scoreValue;

	//menu
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject playPanel;
	[SerializeField] private GameObject aboutPanel;

	//play
	[SerializeField] private GameObject normalPanel;
	[SerializeField] private GameObject hardPanel;
	[SerializeField] private GameObject generatePanel;

	private void Start()
	{
		activePanel = menuPanel;
	}

	public void SetPlayerComponent(GameObject go)
	{
		player = go.GetComponent<PlayerController>();
	}
	public GameObject GetScoreValue()
	{
		return scoreValue;
	}


	public void BackAction(GameObject prevOW)
	{
		prevPanel.SetActive(true);
		activePanel.SetActive(false);

		activePanel = prevPanel;

		prevPanel = prevOW;
	}


	//Gameplay
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


	//Pause
	public void PauseAction()
	{
		prevPanel = gameplayoverlayPanel;

		pausePanel.SetActive(true);
		gameplayoverlayPanel.SetActive(false);

		activePanel = pausePanel;
	}


	//Endgame
	public void EndGameAction()
	{
		prevPanel = gameplayoverlayPanel;

		pausePanel.SetActive(true);
		gameplayoverlayPanel.SetActive(false);

		activePanel = pausePanel;
	}


	//Main Menu
	public void PlayAction()
	{
		prevPanel = mainPanel;

		playPanel.SetActive(true);
		mainPanel.SetActive(false);

		activePanel = playPanel;
	}

	public void AboutAction()
	{
		prevPanel = mainPanel;

		aboutPanel.SetActive(true);
		mainPanel.SetActive(false);

		activePanel = aboutPanel;
	}


	//Play
	public void NormalAction()
	{
		prevPanel = playPanel;

		normalPanel.SetActive(true);
		playPanel.SetActive(false);

		activePanel = normalPanel;
	}

	public void HardAction()
	{
		prevPanel = playPanel;

		hardPanel.SetActive(true);
		playPanel.SetActive(false);

		activePanel = hardPanel;
	}

	public void GenerateAction()
	{
		prevPanel = playPanel;

		generatePanel.SetActive(true);
		playPanel.SetActive(false);

		activePanel = generatePanel;
	}

	public void StartLevel(int lvlNumber, bool normal)
	{
		gameplayMazeRenderer.GetComponent<MazeRenderer>().StartGame(lvlNumber, normal);

		prevPanel = menuPanel;

		gameplayPanel.SetActive(true);
		menuPanel.SetActive(false);

		activePanel = gameplayPanel;
	}
}