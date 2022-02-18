using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailGenerator : MonoBehaviour
{
	[SerializeField] private float PopTime = 0.5f;

	[SerializeField] private float trailPointMinScale = 0.1f;
	[SerializeField] private float trailPointMaxScale = 0.25f;

	[SerializeField] private bool maxScaleOnAllPoints = false;

	public List<Transform> trailPoints;

	public int pointAmoutToShow = 10;

	private int currentIndex = 0;
	private int oldIndex = 9;

	private bool allPointVisible = false;


	private void Start()
	{
		//Hide all points
		foreach(Transform trailPoint in trailPoints)
		{
			trailPoint.localScale = Vector3.zero;
		}
	}

	public void AddPoint()
	{
		//loop List

		//Example:
		//PopIn:   0  1  2  3  4  5  0  1  2  3  4  5  
		//PopOut: -5 -4 -3 -2 -1  0  1  2  3  4  5  0
		oldIndex = currentIndex != pointAmoutToShow - 1 ? currentIndex + 1 : 0;

		if(maxScaleOnAllPoints)
		{
			//Change scale only on newest and oldest points

			Transform oldTrailPoint = trailPoints[oldIndex];
			Transform newTrailPoint = trailPoints[currentIndex];

			newTrailPoint.position = transform.position;

			StartCoroutine(SetScaleCoroutine(newTrailPoint, GetPointScale(currentIndex)));

			if(allPointVisible)
			{
				StartCoroutine(SetScaleCoroutine(oldTrailPoint, GetPointScale(oldIndex)));
			}
		}
		else
		{
			//Change scale on all visible points

			int amount = allPointVisible ? pointAmoutToShow : currentIndex + 1;
			for (int i = 0; i < amount; i++)
			{
				Transform trailPoint = trailPoints[i];

				if (i == currentIndex)
				{
					trailPoint.position = transform.position;
				}

				StartCoroutine(SetScaleCoroutine(trailPoint, GetPointScale(i)));
			}
		}

		if (currentIndex == pointAmoutToShow - 1)
		{
			allPointVisible = true;
			currentIndex = 0;
		}
		else
		{
			currentIndex++;
		}
	}

	private float GetPointScale(int pointIndex)
	{
		float scale = 0;

		if(maxScaleOnAllPoints)
		{
			if(pointIndex != oldIndex)
			{
				scale = trailPointMaxScale;
			}
			else
			{
				scale = 0;
			}
		}
		else
		{
			if (pointIndex != oldIndex)
			{
				//Calculate scale for point based on how old this point is

				//Example:
				//PointsIndexes: 0 - 9
				//currentIndex: 3     3     3    3    3    3
				//pointIndex:   0     1     2    3    4    5
				//Val:         0.7   0.8   0.9   1    0   0.1 
				//Out:         0.7   0.8   0.9   1    0   0.1  

				//currentIndex:  0  1  2  3  4  5  0  1  2  3  4  5  
				//oldIndex:     -5 -4 -3 -2 -1  0  1  2  3  4  5  0 

				float fraction = 0;
				if(currentIndex < pointIndex)
				{
					fraction = 1 - ((float)(currentIndex - (pointIndex - (pointAmoutToShow + 1))) / pointAmoutToShow);
				}
				else
				{
					fraction = 1 - ((float)(currentIndex - pointIndex) / pointAmoutToShow);
				}

				//Scale this point between min and max point scale
				scale = ExtendedMathf.MapFrom01(fraction, trailPointMinScale, trailPointMaxScale);
			}
			else
			{
				scale = 0;
			}
		}

		return scale;
	}

	private IEnumerator SetScaleCoroutine(Transform trailPoint, float endScale)
	{
		float t = 0f;

		float startScale = trailPoint.localScale.x;

		float newScale;

		while (t < 1)
		{
			t += Time.deltaTime / PopTime;

			newScale = Mathf.Lerp(startScale, endScale, t);

			trailPoint.localScale = new Vector3(newScale, newScale, newScale);

			yield return null;
		}
	}


	public void Colorize(int seed)
	{
		foreach(Transform trailPoint in trailPoints)
		{
			trailPoint.GetComponent<ColorRandomizer>().RandomizeColor(seed);
		}
	}
}
