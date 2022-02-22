using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEnabler : MonoBehaviour
{
	private Button button;
	[SerializeField] private TextMeshProUGUI text;

	[SerializeField] private Color activeColor;
	[SerializeField] private Color deactivatedColor;

	private void Awake()
	{
		button = GetComponent<Button>();
		UpdateColor();
	}

	public void UpdateColor()
	{
		if (button.interactable)
		{
			text.color = activeColor;
		}
		else
		{
			text.color = deactivatedColor;
		}
	}
}
