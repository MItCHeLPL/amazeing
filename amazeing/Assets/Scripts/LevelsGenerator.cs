using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelsGenerator : MonoBehaviour
{
    /*This script fills ui scrollview with generated level buttons*/

    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private int levelAmount = 12; //Total lvl amount to generate

    [SerializeField] private int columnAmount = 4;

    [SerializeField] private int spaceBetweenButtons = 250;

    [SerializeField] private int buttonWidth = 150;

    [SerializeField] private int baseYOffset = 50;

    [SerializeField] private bool normalMode = true; //generate buttons that start lvl in hard or normal mode

    [SerializeField] private UIController ui;

    /*Default Settings:
    250 between buttons
    150 height
    1080 screen width*/

    void Start()
    {
        //set scrollview height to fit all buttons inside
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        int height = baseYOffset + ((levelAmount / columnAmount) + 1) * spaceBetweenButtons;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

        GenerateLevels();
    }

    private void GenerateLevels()
	{
        for(int i=0; i<(levelAmount/columnAmount) + 1; i++)
		{
            for(int j=0; j<columnAmount; j++)
			{
                int lvlIndex = (i * columnAmount) + j + 1;

                if (lvlIndex <= levelAmount)
                {
                    int posX = columnAmount % 2 == 0 ? (spaceBetweenButtons / 2) + spaceBetweenButtons * (columnAmount / 2) * -1 + (spaceBetweenButtons * j) : (buttonWidth / 2) + spaceBetweenButtons * (columnAmount / 2) * -1 + (spaceBetweenButtons * j);

                    int posY = -baseYOffset - (spaceBetweenButtons * i);

                    GenerateButton(lvlIndex, posX, posY);
                }
			}
		}
	}

    private void GenerateButton(int lvlNumber, int x, int y)
	{
        GameObject obj = Instantiate(buttonPrefab, transform);
        RectTransform rect = obj.GetComponent<RectTransform>();
        Button button = obj.GetComponent<Button>();
        TextMeshProUGUI text = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        //506.5f is static value to fix X offset
        rect.localPosition = new Vector3(x + 506.5f, y, 0);

        text.SetText(lvlNumber.ToString());

        button.onClick.AddListener(() => ui.StartLevel(lvlNumber, levelAmount, normalMode));
    }
}
