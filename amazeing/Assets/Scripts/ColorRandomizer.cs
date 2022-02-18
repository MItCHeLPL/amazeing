using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorRandomizer : MonoBehaviour
{
    private SpriteRenderer sr;

    //Cap color so it isn't too bright
    [SerializeField] [Range(0,1)] private float colorCap = .75f;

    private Color color;

    [SerializeField] private bool randomizeOnStart = false;

	private void Start()
	{
	    if(randomizeOnStart)
		{
            RandomizeColor();
		}
	}

	public void RandomizeColor()
    {
        sr = GetComponent<SpriteRenderer>();
        color = new Color(0, 0, 0, sr.color.a); //base color

        color.r = Random.Range(0, colorCap); //random chanell consistent between walls

        color.g = Random.Range(0, colorCap);

        color.b = Random.Range(0, colorCap);

        sr.color = color; //assign color
    }
    public void RandomizeColor(int seed)
	{
        sr = GetComponent<SpriteRenderer>();
        color = new Color(0, 0, 0, sr.color.a); //base color

        Random.InitState(seed); //seed
        color.r = Random.Range(0, colorCap); //random chanell consistent between walls

        Random.InitState(seed * 2);
        color.g = Random.Range(0, colorCap);

        Random.InitState(seed * 3);
        color.b = Random.Range(0, colorCap);

        sr.color = color; //assign color
    }
}
