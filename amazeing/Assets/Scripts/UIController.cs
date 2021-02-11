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

	[SerializeField] private GameObject scoreValue;
	[SerializeField] private GameObject endgamescoreValue;
	[SerializeField] private GameObject pausescoreValue;

	//menu
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject playPanel;
	[SerializeField] private GameObject aboutPanel;
	[SerializeField] private GameObject menuBackground;

	//play
	[SerializeField] private GameObject normalPanel;
	[SerializeField] private GameObject hardPanel;
	[SerializeField] private GameObject generatePanel;


	//Set player component on runtime after player is generated
	public void SetPlayerComponent(GameObject go)
	{
		player = go.GetComponent<PlayerController>();
	}

	//Get score value tmpro
	public GameObject GetScoreValue()
	{
		return scoreValue;
	}
	public GameObject GetEndScoreValue()
	{
		return endgamescoreValue;
	}
	public GameObject GetPauseScoreValue()
	{
		return pausescoreValue;
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



	//Gameplay controls
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

	public void StartLevel(int lvlNumber, bool normal)
	{
		gameplayMazeRenderer.GetComponent<MazeRenderer>().StartGame(lvlNumber, normal);

		EnablePanel(gameplayPanel);
		EnablePanel(gameplayoverlayPanel);
		DisablePanel(menuPanel);
		DisablePanel(mainPanel);
		DisablePanel(menuBackground);
	}

	public void StartNextLevel()
	{
		int lvl = gameplayMazeRenderer.GetComponent<MazeRenderer>().seed;
		bool normal = gameplayMazeRenderer.GetComponent<MazeRenderer>().normalMode;

		ClearLevel();

		StartLevel(lvl+1, normal);
	}

	public void RestartLevel()
	{
		int lvl = gameplayMazeRenderer.GetComponent<MazeRenderer>().seed;
		bool normal = gameplayMazeRenderer.GetComponent<MazeRenderer>().normalMode;

		ClearLevel();

		StartLevel(lvl, normal);
	}

	public void ClearLevel()
	{
		gameplayMazeRenderer.GetComponent<MazeRenderer>().ClearGame();

		//Clear score counters
		scoreValue.GetComponent<TextMeshProUGUI>().SetText("0");
		endgamescoreValue.GetComponent<TextMeshProUGUI>().SetText("0");
	}
}