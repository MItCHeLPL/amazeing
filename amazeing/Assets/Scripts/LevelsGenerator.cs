using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelsGenerator : MonoBehaviour
{
    /*This script fills ui scrollview with generated level buttons*/

    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private int columnAmount = 4;

    [SerializeField] private int spaceBetweenButtons = 250;

    [SerializeField] private int buttonWidth = 150;

    [SerializeField] private int baseYOffset = 50;

    [SerializeField] private int splitEvery = 100;

    [SerializeField] private bool normalMode = true; //generate buttons that start lvl in hard or normal mode

    [SerializeField] private bool disableLockedLvls = true;

    [SerializeField] private GameMenager gameMenager;
    [SerializeField] private UIController ui;

    /*Default Settings:
    250 between buttons
    150 height
    1080 screen width*/

    void Awake()
    {
        GenerateLevels();
    }

	private void OnEnable()
	{
        //If finished all from splitEvery generate more levels
        if (gameMenager.lastUnlockedLvl % splitEvery == 0 || gameMenager.lastRaceUnlockedLvl % splitEvery == 0)
		{
            DeleteButtons();
            GenerateLevels();
        }
        else
		{
            UpdateLockState();
        }
    }

	private void GenerateLevels()
	{
        //Show only unlocked levels + rest to splitEvery
        int amountToDisplay = gameMenager.lvlCount;
        if (normalMode)
        {
            amountToDisplay = (gameMenager.lastUnlockedLvl / splitEvery) * splitEvery;
        }
        else
        {
            amountToDisplay = (gameMenager.lastRaceUnlockedLvl / splitEvery) * splitEvery;
        }
        amountToDisplay += splitEvery;
        amountToDisplay = Mathf.Clamp(amountToDisplay, splitEvery, gameMenager.lvlCount);

        //set scrollview height to fit all buttons inside
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        int height = (baseYOffset * 2) + (amountToDisplay / columnAmount) * spaceBetweenButtons;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

        //Generate
        //Y
        for (int i=0; i<(amountToDisplay / columnAmount) + 1; i++)
		{
            //X
            for(int j=0; j<columnAmount; j++)
			{
                int lvlIndex = (i * columnAmount) + j + 1;

                if (lvlIndex <= amountToDisplay)
                {
                    int posX = columnAmount % 2 == 0 ? 
                        (spaceBetweenButtons / 2) + spaceBetweenButtons * (columnAmount / 2) * -1 + (spaceBetweenButtons * j) : 
                        (buttonWidth / 2) + spaceBetweenButtons * (columnAmount / 2) * -1 + (spaceBetweenButtons * j);

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

        //Add onclick to start game at each button
        button.onClick.AddListener(() => gameMenager.StartGame(lvlNumber, normalMode));

        if(disableLockedLvls)
		{
            if (normalMode)
            {
                button.interactable = lvlNumber <= gameMenager.lastUnlockedLvl;
            }
            else
            {
                button.interactable = lvlNumber <= gameMenager.lastRaceUnlockedLvl;
            }

            button.GetComponent<ButtonEnabler>().UpdateColor();
        }
    }

    private void UpdateLockState()
    {
        if (disableLockedLvls)
        {
            int amount = normalMode ? gameMenager.lastUnlockedLvl : gameMenager.lastRaceUnlockedLvl;

            for (int i = 0; i < amount; i++)
            {
                Button button = transform.GetChild(i).GetComponent<Button>();

                if(normalMode)
				{
                    button.interactable = (i + 1) <= gameMenager.lastUnlockedLvl;
                }
                else
				{
                    button.interactable = (i + 1) <= gameMenager.lastRaceUnlockedLvl;
                }

                button.GetComponent<ButtonEnabler>().UpdateColor();
            }
        }
	}

    private void DeleteButtons()
	{
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
