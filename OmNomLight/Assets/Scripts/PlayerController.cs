using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {
    public bool canMove;
    public float initialSpeed;
    public float minSpeed;

    //appearance related
    public Sprite up, down, left, right;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D controller;
    private Vector2 moveThisFrame;
    private float lightBrakeFactor = 0f;

	// Use this for initialization
	void Awake () {
        controller = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        resolveValidPosition();

        move = move * (initialSpeed * (1 - lightBrakeFactor) + minSpeed * lightBrakeFactor);

        if (canMove)
        {
            moveThisFrame = move;
        }
        else
        {
            moveThisFrame = Vector2.zero;
        }
        
	}

    void FixedUpdate() 
    {
        controller.velocity = moveThisFrame;
        selectCorrectSprite();
    }

    void resolveValidPosition()
    {
        //determine if lit
        bool isLit = LightSource.isLit(transform.position);
       
        //determine color
        Color targetColor;
        float targetBrakeFactor = 0f;
        if(CompareTag("Monster"))
        {
            targetColor = isLit ? Color.blue : Color.white;
            targetBrakeFactor = isLit ? 1 : 0;
        }
        else// if (CompareTag("Human"))
        {
            targetColor = isLit ? Color.white : Color.red;
            targetBrakeFactor = isLit ? 0 : 1;
        }

        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * 3);

        //slow down character TODO lerp faster
        lightBrakeFactor = Mathf.Lerp(lightBrakeFactor, targetBrakeFactor, Time.deltaTime * 10);
    }

    void selectCorrectSprite()
    {
        //dont change sprite if not moving
        if (moveThisFrame == Vector2.zero)
        {
            return;
        }
        else
        {
            float horizontalDirection = Mathf.Sign(moveThisFrame.x);

            float angle = Vector2.Angle(Vector2.up, moveThisFrame.normalized);
            //Debug.Log(horizontalDirection * angle);

            if (angle < 20.0f)
            {
                spriteRenderer.sprite = up;
            }
            else if (angle >= 20.0f && angle <= 160.0f)
            {
                spriteRenderer.sprite = (horizontalDirection == -1.0f) ? left : right;
            }
            else
            {
                spriteRenderer.sprite = down;
            }
 
        }
    }
}
