using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveMultiplier = 1.0f; //Distance to move per movement call

    [SerializeField] private LayerMask blockLayer; //Layer to colide against

    private UIController ui;
    private MazeRenderer mazeRenderer;
    private GameManager gameMenager;

    [Header("Player GFX")]
    [SerializeField] private Transform GFX; 
    [SerializeField] private float animSpeed = 0.15f;

    private float currentAngle = 180;

    //GFX coroutines
    private IEnumerator MoveAnimCoroutine;
    private bool moveAnimationPlaying = false;

    //Trail
    private TrailGenerator trailGenerator;
    [SerializeField] private bool trailLengthBasedOnMazeSize = true;

    //Audio
    private AudioController audioController = null;
    [SerializeField] private string audioOnMove = "";

    private void Start()
	{
        ui = GameObject.Find("UI").GetComponent<UIController>();
        ui.SetPlayerComponent(gameObject);

        mazeRenderer = GetComponentInParent<MazeRenderer>();

        //Get trail and set amount of points in trail based on maze size
        trailGenerator = GetComponent<TrailGenerator>();
        if(trailLengthBasedOnMazeSize)
		{
            trailGenerator.pointAmoutToShow = ExtendedMathf.Map(mazeRenderer.mazeSize, mazeRenderer.minMazeSize, mazeRenderer.maxMazeSize, 3, trailGenerator.trailPoints.Count);
        }

        //set same color as maze to player and trail
        if(mazeRenderer.colorize)
		{
            GFX.GetComponent<ColorRandomizer>().RandomizeColor(mazeRenderer.mazeSeed);

            trailGenerator.Colorize(mazeRenderer.mazeSeed);
        }

        gameMenager = ui.gameMenager;

        audioController = gameMenager.audioController;
    }

    public void MovePlayer(Vector2 dir)
	{
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .75f, blockLayer); //collision detection

        if (hit.collider == null) //when player is free to move
		{
            MakeMove(dir);
        }
        else if(hit.collider.CompareTag("Finish"))
		{
            gameMenager.StopGame(true);
		}
        else if (hit.collider.CompareTag("Key"))
        {
            //Move into key position
            MakeMove(dir);  

            //Play Key animation
            mazeRenderer.key.GetComponent<KeyAnimation>().PlayAnimation();
        }
    }

    private void MakeMove(Vector2 dir)
	{
        //Trail
        trailGenerator.AddPoint();

        //Move player
        transform.localPosition += new Vector3(dir.x * moveMultiplier, -dir.y * moveMultiplier, 0); //Move

        //Score
        gameMenager.ChangeScore(gameMenager.score += 1);

        //Audio
        audioController.Play(audioOnMove);

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

        //Rotate towards move direction
        StartCoroutine(RotateAnimation(dir));
    }

    private IEnumerator MoveAnimation(Vector2 endPos)
	{
        moveAnimationPlaying = true;

        float t = 0f;

        Vector2 startPos = GFX.position;

        Vector2 newPos;

        while (t < 1)
        {
            t += Time.deltaTime / animSpeed;

            newPos = Vector2.Lerp(startPos, endPos, t);
 
            GFX.position = newPos;

            yield return null;
        }

        moveAnimationPlaying = false;
    }

    private IEnumerator RotateAnimation(Vector2 dir)
    {
        float t = 0f;

        //Angle towards direction with offset
        float endAngle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90.0f;

        float newAngle;

        while (t < 1)
        {
            t += Time.deltaTime / animSpeed;

            newAngle = Mathf.LerpAngle(currentAngle, endAngle, t);

            GFX.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle)); //Rotate player towards direction

            yield return null;
        }

        GFX.rotation = Quaternion.Euler(new Vector3(0, 0, endAngle));
        currentAngle = endAngle;
    }
}
