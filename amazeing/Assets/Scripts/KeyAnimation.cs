using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimation : MonoBehaviour
{
    private MazeRenderer mazeRenderer;

	private void Start()
	{
        mazeRenderer = GetComponentInParent<MazeRenderer>();
	}

	public void PlayAnimation(float speed)
	{
        StartCoroutine(MoveAnimation(mazeRenderer.finish.position, speed));
	}

    private IEnumerator MoveAnimation(Vector2 endPos, float speed)
    {
        //smooth movement to end position until close to the position
        while (Vector2.Distance(transform.position, endPos) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, endPos, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }

        //Play ending animation

        mazeRenderer.UnlockFinish(); //Unlock mazes finish line

        Autodestruction(); //Destroy after key got to finish later add this as animation event
    }

    public void Autodestruction()
	{
        Destroy(gameObject); 
    }
}
