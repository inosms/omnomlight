using UnityEngine;
using System.Collections;

public class TriggerField : MonoBehaviour 
{
	public bool humanIsHere = false;
	public bool monsterIsHere = false;

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.CompareTag("Human"))
		{
			c.GetComponent<PickUp>().isStandingInFrontOfFridge = true;
			humanIsHere = true;
		}
		else if( c.CompareTag("Monster"))
		{
			monsterIsHere = true;
		}
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if(c.CompareTag("Human"))
		{
			c.GetComponent<PickUp>().isStandingInFrontOfFridge = false;
			humanIsHere = false;
		}
		else if( c.CompareTag("Monster"))
		{
			monsterIsHere = false;
		}
	}
}
