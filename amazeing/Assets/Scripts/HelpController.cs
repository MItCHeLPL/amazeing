using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpController : MonoBehaviour
{
	[SerializeField] private MazeRenderer mazeRenderer;

	private Button button;
	[SerializeField] private Image image;

	[SerializeField] private Color activeColor;
	[SerializeField] private Color deactivatedColor;

	[SerializeField] private int uses = 1;

	private int usesLeft = 1;

	private void Start()
	{
		button = GetComponent<Button>();

		Reset();
	}

	public void ShowPath()
	{
		if(usesLeft > 0)
		{
			var ai = mazeRenderer.ai.GetComponent<AIController>();

			ai.ShowPath();

			usesLeft--;
		}

		if(usesLeft <= 0)
		{
			DisableButton();
		}
	}

	public void Reset()
	{
		EnableButton();

		usesLeft = uses;
	}

	public void DisableButton()
	{
		button.interactable = false;
		image.color = deactivatedColor;
	}
	public void EnableButton()
	{
		button.interactable = true;
		image.color = activeColor;
	}
}
