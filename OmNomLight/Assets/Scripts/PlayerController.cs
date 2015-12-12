using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public bool canMove;
    public float speed;

    private Rigidbody2D rigidbody;
    private Vector2 moveThisFrame;

	// Use this for initialization
	void Awake () {
        rigidbody = GetComponent<Rigidbody2D>();
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
        rigidbody.velocity = moveThisFrame;
    }
}
