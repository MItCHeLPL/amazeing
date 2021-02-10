using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveMultiplier = 1.0f;

    [SerializeField]
    private LayerMask blockLayer;

    //UI
    private UIController ui;
    private TextMeshProUGUI scoreValueTMPro;
    private TextMeshProUGUI endgamescoreValueTMPro;
    private TextMeshProUGUI pausescoreValueTMPro;

    public int score = 0;

	private void Start()
	{
       ui = GameObject.Find("UI").GetComponent<UIController>();
       ui.SetPlayerComponent(gameObject);
       scoreValueTMPro = ui.GetScoreValue().GetComponent<TextMeshProUGUI>();
       endgamescoreValueTMPro = ui.GetEndScoreValue().GetComponent<TextMeshProUGUI>();
       pausescoreValueTMPro = ui.GetPauseScoreValue().GetComponent<TextMeshProUGUI>();
    }

    public void MovePlayer(Vector2 dir)
	{
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .75f, blockLayer); //detect colisions

        if (hit.collider == null) //when player is free to move
		{
            transform.localPosition += new Vector3(dir.x * moveMultiplier, 0, -dir.y * moveMultiplier);
            ChangeScoreBy(1);
        }
        else if(hit.collider.tag == "Finish") //when player crosses the finish line
		{
            ui.EndGameAction();
            Debug.Log("Win");
		}
        else
		{
            Debug.Log(hit.collider.tag); //when player hits something
        }
	}

    private void ChangeScoreBy(int x)
	{
        //Change score
        score += x; 

        //Change Ui
        scoreValueTMPro.SetText(score.ToString()); 
        endgamescoreValueTMPro.SetText(score.ToString());
        pausescoreValueTMPro.SetText(score.ToString());
    }
}
