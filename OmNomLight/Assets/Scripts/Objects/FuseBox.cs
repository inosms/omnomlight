using UnityEngine;
using System.Collections;

public class FuseBox : MonoBehaviour 
{
	private TriggerField triggerField;
	public bool isActivated = false;

	void Start()
	{
		triggerField = GetComponentInChildren<TriggerField>();
	}

	void Update()
	{


        if ((Input.GetButtonDown("Pick Up") && triggerField.humanIsHere)  
            || (Input.GetButtonDown("Pick Up Controller") && triggerField.monsterIsHere)) 
		{
			// if somebody is at the fuse box, the action is the change the current state
			isActivated = !isActivated;
		}
	}
}
