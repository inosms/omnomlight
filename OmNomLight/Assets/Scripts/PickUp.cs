﻿using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public float range = 0.5f;
	public LayerMask layerMask;

    bool carrying = false;
    private GetCarried carriedObject;
	public bool isStandingInFrontOfFridge = false;

    void Update()
    {
		
		if(Input.GetButtonDown("Pick Up") || Input.GetButtonDown("Pick Up Controller"))
        {
            if(!carrying)//currently not carrying anything
            {
				RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector3.back,range,layerMask);

                //check the pickup zone
                if (hit)
                {
                    carriedObject = hit.collider.GetComponent<GetCarried>();

                    Debug.Log(hit.collider.gameObject.name);

                    //an object that can be carried is inside the pickup zone
                    if (carriedObject)
                    {
						if( gameObject.CompareTag("Monster") && Input.GetButtonDown("Pick Up Controller"))
							MonsterAction(carriedObject);
						else if(gameObject.CompareTag("Human") && Input.GetButtonDown("Pick Up") )
							HumanAction(carriedObject);
                    }
                }
            }
            else if (carriedObject)
            {
				// only drop the object when not directly in front of fridge,
				// as one wants to put the food into the fridge in this case
				if( !isStandingInFrontOfFridge )
				{
					StopCarrying();
				}
            }
        }
    }

	private void MonsterAction(GetCarried n_object)
	{
		Debug.Log("Monster Action");
		Collectible tmp_food = n_object.GetComponent<Collectible>();

		// only if the object really is food; prevent eating the Candle
		if( n_object.GetComponent<Collectible>() != null )
		{
			// Play on random music clip
			int tmp_random = Random.Range(0,4);
			if( tmp_random == 0 )
				gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating1;
			else if( tmp_random == 1)
				gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating2;
			else if( tmp_random == 2 )
				gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating3;
			else
				gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating4;
			gameObject.GetComponent<AudioSource>().Play();

			// When Human holds the object, let the monster not eat stuff
			if( GameObject.FindObjectOfType<PickUp>().GetCarriedObject() != this.GetComponent<GetCarried>() )
			{
				GameObject.FindObjectOfType<GameState>().MonsterEatsOneFood();

				GameObject.FindObjectOfType<PickUp>().StopCarrying();

				n_object.gameObject.SetActive(false);

				Destroy(n_object.gameObject, 0.3f);
			}
		}
	}

	private void HumanAction(GetCarried n_object)
	{
		GetComponent<AudioSource>().clip = GetComponent<PlayerController>().audioPickUp;
		GetComponent<AudioSource>().Play();

		carriedObject.startCarrying(transform);
		carrying = true;
	}

	public void StopCarrying()
	{
		//drop the carried object
		carriedObject.stopCarrying();
		carriedObject = null;
		carrying = false;
	}


	public GetCarried GetCarriedObject()
	{
		return carriedObject;
	}

}
