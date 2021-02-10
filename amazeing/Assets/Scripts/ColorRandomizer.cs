using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorRandomizer : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField]
    [Range(0,1)]
    private float colorCap = .7f;

    private Color color;

    public void RandomizeColor(int seed)
	{
        sr = GetComponent<SpriteRenderer>();
        color = new Color(0, 0, 0, 1); //base color

        Random.InitState(seed); //seed
        color.r = Random.Range(0, colorCap); //random chanell consistent between walls

        Random.InitState(seed * 2);
        color.g = Random.Range(0, colorCap);

        Random.InitState(seed * 3);
        color.b = Random.Range(0, colorCap);

        sr.color = color; //assign color
    }
}
