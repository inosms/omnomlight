using UnityEngine;
using System.Collections;
// http://answers.unity3d.com/questions/176753/c-arraylist.html
using System.Collections.Generic;

public class Fridge : MonoBehaviour {


	private List<Collectible> m_foodList = new List<Collectible>();
	private LightSource m_lightSource;
	private FridgeTriggerField triggerField;

	private GameObject monster;
	private GameObject human;

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
		triggerField = GetComponentInChildren<FridgeTriggerField>();

		monster = GameObject.FindWithTag("Monster");
		human = GameObject.FindWithTag("Human");
	}


	// Update is called once per frame
	void Update () 
	{
		// update light source
		// FIXME: remove m_fuseBox; this only as long as this is not initialized
		if( m_fuseBox && m_fuseBox.isActivated && m_doorState == DoorState.OPEN )
			m_lightSource.SetIsOn(true);
		else
			m_lightSource.SetIsOn(false);

		if( m_doorState == DoorState.OPEN)
			m_lightSource.SetIsOn(true);
		else
			m_lightSource.SetIsOn(false);


		if (Input.GetButtonDown ("Pick Up")) 
		{
			// if human is at the fridge
			if( triggerField.humanIsHere )
				ActionHuman();
			// else if monster is at the fridge
			else if( triggerField.monsterIsHere )
				ActionMonster();
		}
	}

	private void ActionHuman()
	{
		// When door is closed, open the door
		if( m_doorState == DoorState.CLOSED )
			m_doorState = DoorState.OPEN;
		// Otherwise put the food in
		else
		{
			// When the Human just carries something put it in the fridge
			PickUp tmp_pickUp = human.GetComponent<PickUp>();

			if( tmp_pickUp.GetCarriedObject() != null )
			{
				Collectible tmp_foodThing = tmp_pickUp.GetCarriedObject().GetComponent<Collectible>();
				// http://answers.unity3d.com/questions/7776/how-to-make-an-gameobject-invisible-and-disappeare.html
				// make object invisible
				tmp_foodThing.gameObject.SetActive(false);
				m_foodList.Add(tmp_foodThing);
				Debug.Log("Added thing to fridge");
				tmp_pickUp.StopCarrying();
			}
			// otherwise close the door
			else
			{
				m_doorState = DoorState.CLOSED;
			}
		}
	}

	private void ActionMonster()
	{
		if( m_doorState == DoorState.CLOSED )
			m_doorState = DoorState.OPEN;
		else
		{
			// Eat something 
		}
	}
}
