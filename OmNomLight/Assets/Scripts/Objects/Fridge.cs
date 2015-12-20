using UnityEngine;
using System.Collections;
// http://answers.unity3d.com/questions/176753/c-arraylist.html
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Fridge : MonoBehaviour {


	private List<Collectible> m_foodList = new List<Collectible>();
	private LightSource m_lightSource;
	private TriggerField triggerField;
	private SpriteRenderer spriteRenderer;
	private AudioSource audioSource;

	public AudioClip audioFridgeOpen;
	public AudioClip audioFridgeClose;
	public AudioClip audioPutInFridge;
	public AudioClip audioEmptyFridge;

	public Sprite fridgeClosed;
	public Sprite fridgeOpenedEmpty;
	public Sprite fridgeOpenedFull;

	private GameObject human;
	private GameState gameState;

	public FuseBox m_fuseBox;

	enum DoorState 
	{
		OPEN,
		CLOSED
	}
	private DoorState m_doorState = DoorState.CLOSED;

	void Start()
	{
		m_lightSource = GetComponentInChildren<LightSource>();
		triggerField = GetComponentInChildren<TriggerField>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();

		human = GameObject.FindWithTag("Human");
		m_fuseBox = GameObject.FindObjectOfType<FuseBox>();
		gameState = GameObject.FindObjectOfType<GameState>();
	}


	// Update is called once per frame
	void Update () 
	{
		// update light source
		if( m_fuseBox && m_fuseBox.isActivated && m_doorState == DoorState.OPEN )
			m_lightSource.SetIsOn(true);
		else
			m_lightSource.SetIsOn(false);

		if( m_doorState == DoorState.OPEN && m_foodList.Count == 0 )
			spriteRenderer.sprite = fridgeOpenedEmpty;
		else if( m_doorState == DoorState.OPEN && m_foodList.Count > 0 )
			spriteRenderer.sprite = fridgeOpenedFull;
		else
			spriteRenderer.sprite = fridgeClosed;



			// if human is at the fridge
        if (triggerField.humanIsHere && Input.GetButtonDown("Pick Up"))
			ActionHuman();
			// else if monster is at the fridge
        else if (triggerField.monsterIsHere && Input.GetButtonDown("Pick Up Controller"))
			ActionMonster();
		
	}

	private void OpenDoor()
	{
		m_doorState = DoorState.OPEN;
		audioSource.clip = audioFridgeOpen;
		audioSource.Play();
	}

	private void CloseDoor()
	{
		m_doorState = DoorState.CLOSED;
		audioSource.clip = audioFridgeClose;
		audioSource.Play();
	}
		
	private void ActionHuman()
	{
		// When door is closed, open the door
		if( m_doorState == DoorState.CLOSED )
			OpenDoor();
		// Otherwise put the food in
		else
		{
			// When the Human just carries something put it in the fridge
			PickUp tmp_pickUp = human.GetComponent<PickUp>();

			if( tmp_pickUp.GetCarriedObject() != null &&  tmp_pickUp.GetCarriedObject().GetComponent<Collectible>() != null )
			{
				Collectible tmp_foodThing = tmp_pickUp.GetCarriedObject().GetComponent<Collectible>();
				// http://answers.unity3d.com/questions/7776/how-to-make-an-gameobject-invisible-and-disappeare.html
				// make object invisible
				tmp_foodThing.gameObject.SetActive(false);
				m_foodList.Add(tmp_foodThing);
				gameState.HumanRefridgeratesOneFood();

				audioSource.clip = audioPutInFridge;
				audioSource.Play();

				//Debug.Log("Added thing to fridge");
				tmp_pickUp.StopCarrying();
			}
			// otherwise close the door
			else
				CloseDoor();
		}
	}

	private void ActionMonster()
	{
		// At first open door if fuse box is deactivated
		if( m_doorState == DoorState.CLOSED /*&& m_fuseBox && m_fuseBox.isActivated == false*/)
		{
			OpenDoor();
			// Empty fridge
			while( m_foodList.Count != 0 )
			{
				Collectible tmp_food = m_foodList[0];
				m_foodList.RemoveAt(0);
				gameState.OneFoodIsRemovedFromFridge();

				audioSource.clip = audioEmptyFridge;
				audioSource.Play();

				tmp_food.gameObject.transform.position += new Vector3(Random.Range(-10,10)/10.0f,-Random.Range(0,10)/10.0f,0f);
				tmp_food.gameObject.SetActive(true);
			}
		}
		// the monster can also close the door
		else if(m_doorState == DoorState.OPEN )
			CloseDoor();
	}
}
