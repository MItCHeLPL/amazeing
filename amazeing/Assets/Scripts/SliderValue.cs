using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValue : MonoBehaviour
{
    public void UpdateSliderValue(TextMeshProUGUI text)
	{
		text.SetText(GetComponent<Slider>().value.ToString());
	}
}
