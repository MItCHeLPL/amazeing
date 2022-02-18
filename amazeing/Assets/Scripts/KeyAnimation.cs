using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimation : MonoBehaviour
{
    private MazeRenderer mazeRenderer;

    [SerializeField] private float animLength = 0.75f;

    [SerializeField] private AnimationCurve speedCurve;

    private void Start()
	{
        mazeRenderer = GetComponentInParent<MazeRenderer>();
	}

	public void PlayAnimation()
	{
        StartCoroutine(MoveAnimation(mazeRenderer.finish.position));
	}

    private IEnumerator MoveAnimation(Vector2 endPos)
    {
        //Move towards finish line
        float t = 0f;

        Vector2 startPos = transform.position;

        Vector2 newPos;

        while (t < 1)
        {
            t += Time.deltaTime / animLength;

            newPos = Vector2.Lerp(startPos, endPos, speedCurve.Evaluate(t));

            transform.position = newPos;

            yield return null;
        }


        //Play ending animation


        mazeRenderer.UnlockFinish();


        Autodestruction();
    }

    public void Autodestruction()
	{
        Destroy(gameObject); 
    }
}
