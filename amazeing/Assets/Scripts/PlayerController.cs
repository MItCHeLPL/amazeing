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
    [SerializeField] private float animSpeed = 0.15f; //playter GFX animation speed

    private float currentAngle = 0;

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
        MoveAnimCoroutine = MoveAnimation(transform.position, dir);
        StartCoroutine(MoveAnimCoroutine);
    }

    private IEnumerator MoveAnimation(Vector2 endPos, Vector2 dir)
	{
        moveAnimationPlaying = true;

        float t = 0f;

        Vector2 startPos = GFX.position;

        //Angle towards direction with offset
        float endAngle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90.0f;

        Vector2 newPos;
        float newAngle;

        while (t < 1)
        {
            t += Time.deltaTime / animSpeed;

            newPos = Vector2.Lerp(startPos, endPos, t);
            newAngle = Mathf.LerpAngle(currentAngle, endAngle, t);
 
            GFX.position = newPos;
            GFX.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));

            yield return null;
        }

        currentAngle = endAngle;

        moveAnimationPlaying = false;
    }
}
