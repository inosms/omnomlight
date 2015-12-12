using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameState : MonoBehaviour {

	private Collectible[] m_collectables;

	private int totalCollectibles;
	private int eatenCollectibles = 0;
	private int refridgeratedCollectibleds = 0;

	enum WinState
	{
		NONE,
		MONSTER,
		HUMAN
	}

	private WinState winState;

	// Use this for initialization
	void Start () 
	{
		totalCollectibles = GameObject.FindObjectsOfType<Collectible>().Length;
	}

	// returns whether the game is won or not
	bool IsOver()
	{
		return winState != WinState.NONE;
	}

	void CheckGameState()
	{
		if( eatenCollectibles > totalCollectibles/2 )
			winState = WinState.MONSTER;
		else if( refridgeratedCollectibleds > totalCollectibles/2 )
			winState = WinState.HUMAN;
	}

	public void MonsterEatsOneFood()
	{
		eatenCollectibles++;
	}

	public void HumanRefridgeratesOneFood()
	{
		refridgeratedCollectibleds++;
	}

	public void OneFoodIsRemovedFromFridge()
	{
		refridgeratedCollectibleds--;
	}

}
