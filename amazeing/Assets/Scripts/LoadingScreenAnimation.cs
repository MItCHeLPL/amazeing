using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreenAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float timeBetweenDots = 0.5f;

    [HideInInspector] public bool finishedAnimationOnece = false;


    private void OnEnable()
	{
        finishedAnimationOnece = false;

        StartCoroutine(LoadingCoroutine());
	}

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator LoadingCoroutine()
    {
        while(isActiveAndEnabled)
		{
            text.SetText(".");

            yield return new WaitForSecondsRealtime(timeBetweenDots);

            text.SetText("..");

            yield return new WaitForSecondsRealtime(timeBetweenDots);

            text.SetText("...");

            yield return new WaitForSecondsRealtime(timeBetweenDots);

            text.SetText("");

            yield return new WaitForSecondsRealtime(timeBetweenDots);

            finishedAnimationOnece = true;
        }
    }
}
