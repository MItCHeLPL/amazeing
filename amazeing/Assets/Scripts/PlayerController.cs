using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveMultiplier = 1.0f;

    [SerializeField] private LayerMask blockLayer;

    public bool hasKey = false;

    private UIController ui;
    private MazeRenderer mazeRenderer;
    private GameMenager gameMenager;

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .75f, blockLayer); //detect colisions

        if (hit.collider == null) //when player is free to move
		{
            MakeMove(dir);
        }
        else if(hit.collider.CompareTag("Finish")) //when player crosses the finish line
		{
            gameMenager.StopGame();
            Debug.Log("Win");
		}
        else if (hit.collider.CompareTag("Key")) //when player collected key
        {
            //Move into key position
            MakeMove(dir);

            hasKey = true;

            //Unlock finish
            mazeRenderer.UnlockFinish();

            //Change UI
            ui.EnablePanel(ui.keyIconEnabled);
            ui.DisablePanel(ui.keyIconDisabled);

            //Destroy key GO
            Destroy(hit.transform.gameObject);

            Debug.Log("Got key");
        }
    }

    private void MakeMove(Vector2 dir)
	{
        //TODO ENABLE MOVE ANIMATION


        transform.localPosition += new Vector3(dir.x * moveMultiplier, 0, -dir.y * moveMultiplier); //Move
        gameMenager.ChangeScore(gameMenager.score += 1); //Add score
    }
}
