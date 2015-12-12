using UnityEngine;
using System.Collections;

public class MonsterWins : MonoBehaviour {

	void Update () 
	{
		if( Input.anyKeyDown )
			Application.LoadLevel("MainMenu");
	}
}