using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameState : MonoBehaviour {

	private Collectible[] m_collectables;

	private int totalCollectibles;
	private int eatenCollectibles = 0;
	private int refridgeratedCollectibleds = 0;

	private bool isOver = false;

	// Use this for initialization
	void Start () 
	{
		totalCollectibles = GameObject.FindObjectsOfType<Collectible>().Length;
	}

	bool IsOver()
	{
		return isOver;
	}

	void CheckGameState()
	{
		//if( )
	}

}
