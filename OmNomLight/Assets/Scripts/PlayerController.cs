using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public bool canMove;
    public float speed;

    private Rigidbody2D controller;
    private Vector2 moveThisFrame;

	// Use this for initialization
	void Awake () {
        controller = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float x = 0;
        float y = 0;
        if (CompareTag("Monster"))
        {
            x = Input.GetAxis("HorizontalController");
            y = Input.GetAxis("VerticalController");
        }
        else if (CompareTag("Human"))
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }
        

        Vector2 move = new Vector2(x, y);
        if (move.magnitude > 1)
        {
            move = move.normalized;
        }

        move = move * speed;

        if (canMove)
        {
            moveThisFrame = move;
        }
        else
        {
            moveThisFrame = Vector2.zero;
        }
        resolveValidPosition();
	}

    void FixedUpdate() 
    {
        controller.velocity = moveThisFrame;
    }

    void resolveValidPosition()
    {
        if(CompareTag("Monster"))
        {
            bool isLit = LightSource.isLit(transform.position);

            GetComponent<Renderer>().materials[0].color = isLit ? Color.red : Color.green;
        }
        else if (CompareTag("Human"))
        {
            bool isLit = LightSource.isLit(transform.position);

            GetComponent<Renderer>().materials[0].color = isLit ? Color.blue : Color.red;
        }
    }
}
