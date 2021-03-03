using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveMultiplier = 1.0f; //Distance to move per movement call

    [SerializeField] private LayerMask blockLayer; //Layer to colide against

    private UIController ui;
    private MazeRenderer mazeRenderer;
    private GameMenager gameMenager;

    [SerializeField] private Transform GFX; //player GFX
    [SerializeField] private float animSpeed = 10.0f; //playter GFX animation speed

    //GFX coroutine
    private IEnumerator MoveAnimCoroutine;
    private bool moveAnimationPlaying = false;

	private void Start()
	{
        //Get UI
        ui = GameObject.Find("UI").GetComponent<UIController>();
        ui.SetPlayerComponent(gameObject);

        mazeRenderer = GetComponentInParent<MazeRenderer>();

        gameMenager = ui.gameMenager;
    }

    public void MovePlayer(Vector2 dir)
	{
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .75f, blockLayer); //collision detection

        if (hit.collider == null) //when player is free to move
		{
            MakeMove(dir);
        }
        else if(hit.collider.CompareTag("Finish")) //when player crosses the finish line
		{
            gameMenager.StopGame();
		}
        else if (hit.collider.CompareTag("Key")) //when player collected key
        {
            //Move into key position
            MakeMove(dir);  

            //Play Key animation
            mazeRenderer.key.GetComponent<KeyAnimation>().PlayAnimation(5.0f);
        }
    }

    private void MakeMove(Vector2 dir)
	{
        transform.localPosition += new Vector3(dir.x * moveMultiplier, -dir.y * moveMultiplier, 0); //Move

        gameMenager.ChangeScore(gameMenager.score += 1); //Add score

        //GFX Animation
        //if animation is played stop it and start new animation to the new position
        if(moveAnimationPlaying)
		{
            StopCoroutine(MoveAnimCoroutine);
            moveAnimationPlaying = false;
        }
        //Start Coroutine
        MoveAnimCoroutine = MoveAnimation(transform.position);
        StartCoroutine(MoveAnimCoroutine);
    }

    private IEnumerator MoveAnimation(Vector2 endPos)
	{
        moveAnimationPlaying = true; //indicator

        //smooth movement to end position until close to the position
        while (Vector2.Distance(GFX.position, endPos) > 0.01f)
		{
            GFX.position = Vector2.Lerp(GFX.position, endPos, Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        GFX.position = endPos; //snap to end position

        moveAnimationPlaying = false; //indicator
    }
}
