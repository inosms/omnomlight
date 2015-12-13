using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameState : MonoBehaviour {

	private Collectible[] m_collectables;

	private int totalCollectibles;
	private int eatenCollectibles = 0;
	private int refridgeratedCollectibleds = 0;

	// Use this for initialization
	void Start () 
	{
		totalCollectibles = GameObject.FindObjectsOfType<Collectible>().Length;
	}
		
	private void CheckGameState()
	{
		if( eatenCollectibles > totalCollectibles/2 )
        {
            Application.LoadLevel("MonsterWins");
            LightSource.lightSources.Clear();
            LightObstacle.obstacles.Clear();
        }

        else if (refridgeratedCollectibleds > totalCollectibles / 2)
        {
            Application.LoadLevel("HumanWins");
            LightSource.lightSources.Clear();
            LightObstacle.obstacles.Clear();
        }
    }

	public void MonsterEatsOneFood()
	{
		eatenCollectibles++;
		CheckGameState();
	}

	public void HumanRefridgeratesOneFood()
	{
		refridgeratedCollectibleds++;
		CheckGameState();
	}

	public void OneFoodIsRemovedFromFridge()
	{
		refridgeratedCollectibleds--;
		CheckGameState();
	}

}
