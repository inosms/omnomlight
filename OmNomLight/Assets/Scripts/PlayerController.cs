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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(x, y);
        if (move.magnitude > 1)
        {
            move = move.normalized;
        }

        move = move * speed;

        moveThisFrame = move;
	}

    void FixedUpdate() 
    {
        controller.velocity = moveThisFrame;
    }

    void resolveValidPosition()
    {
        if(CompareTag("Monster"))
        {
            bool isLit = false;
            foreach(LightSource l in LightSource.lightSources)
            {
                if(l.isLit(transform.position))
                {
                    isLit = true;
                }
            }

            GetComponent<Renderer>().materials[0].color = isLit ? Color.red : Color.green;
        }
        else if (CompareTag("Human"))
        {
            bool isLit = false;
            foreach (LightSource l in LightSource.lightSources)
            {
                if (l.isLit(transform.position))
                {
                    isLit = true;
                }
            }

            GetComponent<Renderer>().materials[0].color = isLit ? Color.blue : Color.red;
        }
    }
}
