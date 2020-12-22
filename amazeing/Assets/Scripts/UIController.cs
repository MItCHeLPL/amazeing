using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
	private PlayerController player;

	public void GetPlayerComponent(GameObject go)
	{
		player = go.GetComponent<PlayerController>();
	}

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

	public void PauseAction()
	{

	}
}
