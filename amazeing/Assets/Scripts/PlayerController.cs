using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private LayerMask blockLayer;

    private Vector2 currentPosition;

    private UIController ui;
    private TextMeshProUGUI scoreValue;

	private void Start()
	{
       ui = GameObject.Find("UI").GetComponent<UIController>();
       ui.SetPlayerComponent(gameObject);
        scoreValue = ui.GetScoreValue().GetComponent<TextMeshProUGUI>();
    }

	void Update()
    {
        currentPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);

        /*if (Mathf.Abs(Input.GetAxis("Horizontal")) == 1.0f || Mathf.Abs(Input.GetAxis("Vertical")) == 1.0f)
		{
            PlayerMove(new Vector2(Input.GetAxis("Horizontal")/2, Input.GetAxis("Vertical")/2));
        }*/
    }

    public void MovePlayer(Vector2 dir)
	{
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .75f, blockLayer);
        if (hit.collider == null)
		{
            transform.localPosition += new Vector3(dir.x, 0, -dir.y);
        }
        else if(hit.collider.tag == "Finish")
		{
            ui.EndGameAction();
            Debug.Log("Win");
		}
        else
		{
            Debug.Log(hit.collider.tag);
        }
	}
}
